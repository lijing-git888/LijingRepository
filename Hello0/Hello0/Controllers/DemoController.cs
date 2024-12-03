using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Hello0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private static readonly string[] Greetings = new[]
        {
            "你好", "Hello", "Hola", "Bonjour", "Ciao"
        };
        private readonly UserRepository _userRepository;

        public DemoController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("Random")]
        public ActionResult<string> GetRandom()
        {
            var rng = new Random();
            return Greetings[rng.Next(Greetings.Length)];
        }

        // 添加用户
        [HttpPost("add")]
        public ActionResult AddUser([FromBody] users user)
        {
            _userRepository.AddUser(user);
            return Ok();
        }

        // 删除用户
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
            return Ok();
        }

        // 更新用户
        [HttpPut("update")]
        public ActionResult UpdateUser([FromBody] users user)
        {
            _userRepository.UpdateUser(user);
            return Ok();
        }

        // 查询所有用户
        [HttpGet("users")]
        public ActionResult<List<users>> GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        // 根据ID查询用户
        [HttpGet("user/{id}")]
        public ActionResult<users> GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            return Ok(user);
        }
    }
}
