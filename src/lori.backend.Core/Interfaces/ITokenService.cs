namespace lori.backend.Core.Interfaces;
public interface ITokenService<T> where T : class
{
  string CreateToken(T login);
}
