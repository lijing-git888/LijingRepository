using SqlSugar;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Hello0
{
    public class UserRepository
    {
        private readonly SqlSugarClient _db;
        private readonly IConnectionMultiplexer _redis;

        public UserRepository(SqlSugarClient db, IConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis;
        }

        // 添加用户
        public void AddUser(users user)
        {
            _db.Insertable(user).ExecuteCommand();
        }

        // 删除用户
        public void DeleteUser(int id)
        {
            _db.Deleteable<users>().Where(u => u.Id == id).ExecuteCommand();
        }

        // 更新用户
        public void UpdateUser(users user)
        {
            _db.Updateable(user).ExecuteCommand();
        }

        // 查询用户
        public List<users> GetUsers()
        {
            return _db.Queryable<users>().ToList();
        }

        // 根据ID查询用户
        public users GetUserById(int id)
        {
            var db = _redis.GetDatabase();
            var cacheKey = $"user:{id}";

            // 尝试从 Redis 中获取用户
            var cachedUser = db.StringGet(cacheKey);
            if (cachedUser.HasValue)
            {
                return JsonConvert.DeserializeObject<users>(cachedUser);
            }

            // 如果 Redis 中没有，查询数据库
            var user = _db.Queryable<users>().InSingle(id);
            if (user != null)
            {
                // 将用户信息存入 Redis
                db.StringSet(cacheKey, JsonConvert.SerializeObject(user), TimeSpan.FromMinutes(30)); // 设置缓存时间
            }

            return user;
        }
    }
}
