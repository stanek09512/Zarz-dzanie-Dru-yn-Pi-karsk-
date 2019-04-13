using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace InzBackCore.Domain
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public string Position { get; protected set; }
        public int Age { get; protected set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public PlayersStatictics Statictics { get; set; }
        public List<MatchhPlayer> Matchhs2Player { get; set; } = new List<MatchhPlayer>();


        public Player()
        { }
        public Player( string Name, string Surname, string Position, int Age)    // nie wiem czy nie protected. sprawdzic
        {
            SetName(Name);
            SetSurname(Surname);
            SetPosition(Position);
            SetAge(Age);
            

        }

        public void SetName(string name)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"Player with id: '{Id}' can not have an empty name.");
            }
            bool isDigitPresentName = name.Any(c => char.IsDigit(c));

            if (isDigitPresentName == true )
            {
                throw new Exception($"Object which you want a create contain digits in Name");
            }

            Name = name;
            //mozna w modelu dodac date modyfikacji i tu ja zmieniac
        }
        public void SetSurname(string surname)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new Exception($"Player with id: '{Id}' can not have an empty surname.");
            }

            bool isDigitPresentSurname = surname.Any(c => char.IsDigit(c));
            if (isDigitPresentSurname == true)
            {
                throw new Exception($"Object which you want a create contain digits in surname");
            }
            Surname = surname;
            //mozna w modelu dodac date modyfikacji i tu ja zmieniac
        }
        public void SetPosition(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                throw new Exception($"Player with id: '{Id}' can not have an empty position.");
            }
            bool isDigitPresentPosition = position.Any(c => char.IsDigit(c));
            if (isDigitPresentPosition == true)
            {
                throw new Exception($"Object which you want a create contain digits in position");
            }
            Position = position;
        }
        public void SetAge(int age)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (age==0)
            {
                throw new Exception($"Player with id: '{Id}' can not have an null or zero.");
            }
            Age = age;
            //mozna w modelu dodac date modyfikacji i tu ja zmieniac
        }

    }
}
