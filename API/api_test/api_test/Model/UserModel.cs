using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Model
{
    public class UserModel
    {
        [JsonProperty("user")]

        public string Name { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avata { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }

    public class UserEdit
    {
    [JsonProperty("user")]
    public string Name { get; set; }
    public int? Gender { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Avata { get; set; }
    public string Username { get; set; }
    }

    public class UserView
    {
        [JsonProperty("user")]
        public string Name { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avata { get; set; }
        public string Username { get; set; }
    }
}