using Authentication.Data;
using Authentication.Entity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Repo;

public class AuthenticationRepository
{

    private UserDbContext context;

    public AuthenticationRepository(UserDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> RegisterAsync(User user)
    {
        var getUser = await GetUserByEmailAsync(user.Email);

        if (getUser is not null) return false;
        
        
        var res=await context.Users.AddAsync(user);

        await context.SaveChangesAsync();

        return res.Entity.Id> 0 ? true : false;
    }

    public async Task<User> GetUserByEmailAsync(string userEmail)
    {
        var user = await context.Users.FirstOrDefaultAsync( u => u.Email == userEmail);
        
        return user is not null ? user : null!;
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);

        if (user is not null && user.Password == password) return user;
        
        return null!;
    }
}