using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InzBackCore.Domain
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }    // to odpowiada za hashowanie hasla do bazy
        public byte[] PasswordSalt { get; set; }    // a tu jeszcze jakis dodatek do hashowania zeby kazde nie bylo tak samo zahashowane jezeli jest takie samo chyba
        public string Role { get; set; }    // rola- uprawnienia

        public List<Player> players { get; set; } = new List<Player>();
        public List<Matchh> matches { get; set; } = new List<Matchh>();
      //  public List<MatchhPlayer> matchesplayers { get; set; } = new List<MatchhPlayer>();


    }
}
