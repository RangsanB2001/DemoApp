using AuthenticationAPI.Models;
using AuthenticationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RangsanContext _dbContext;

        public UserController(RangsanContext dbContext)
        {
            _dbContext = dbContext;
        }
        // สมัครสมาชิก
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest request, CancellationToken cancellationToken = default)
        {
            var user = new Models.User
            {
                username = request.UserName,
                password = request.Password,
            };

            await _dbContext.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }
        // ล็อกอิน
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
        {
            var query = await _dbContext.users
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                x =>
                x.username == request.Username &&
                x.password == request.Password
                , cancellationToken);

            var returnData = new LoginResponse();
            if (query != null)
            {
                returnData.iduser = query.iduser;
                returnData.username = query.username;
            }

            return Ok(returnData);
        }
        // อัปเดต
        [HttpPatch("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] Models.User request, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(request);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(request);
        }
        //ลบ
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] Models.User request, CancellationToken cancellationToken = default)
        {
            _dbContext.Remove(request);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var user = await _dbContext.users.FirstOrDefaultAsync(x => x.iduser == request.iduser, cancellationToken);

            return Ok(user);
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserProfile(int iduser, CancellationToken cancellationToken = default)
        {
            var userData = await _dbContext.users
                 .AsSplitQuery()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.iduser == iduser, cancellationToken);

            if (userData != null)
            {
                var returnData = new GetDataUser();
                returnData.userId = userData.iduser;
                returnData.userName = userData.username;
                returnData.password = userData.password;
                return Ok(returnData);
            }
            return NotFound();
        }

    }
}
