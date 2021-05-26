﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PeliculasApi.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasApi.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager,IConfiguration configuration,SignInManager<IdentityUser> signInManager)
       {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }
       [HttpPost("crear")]
       public async Task<ActionResult<RespuestaAutenticacion>>Crear ([FromBody] CredencialesUsuarios credenciales)
       {
            var usuario = new IdentityUser { UserName = credenciales.Email, Email = credenciales.Email };
            var resultado = await userManager.CreateAsync(usuario, credenciales.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToekn(credenciales);
            }
            else 
            {
                return BadRequest(resultado.Errors);
            } 
                
       }
        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login([FromBody] CredencialesUsuarios credenciales)
        {
            var resultado = await signInManager.PasswordSignInAsync(credenciales.Email, credenciales.Password, isPersistent:false,lockoutOnFailure:false);
            if (resultado.Succeeded)
            {
                return await ConstruirToekn(credenciales);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }
       private async Task<RespuestaAutenticacion>ConstruirToekn(CredencialesUsuarios credenciales)
       {
            var claims = new List<Claim>()
            {
                new Claim("email",credenciales.Email)
            };
            var usuario = await userManager.FindByEmailAsync(credenciales.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion

            };
       }
    }
}
