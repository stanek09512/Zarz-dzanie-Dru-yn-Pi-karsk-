using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InzBackCore.Domain
{
    public class Matchh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public string OpponentTeam { get; protected set; }
        public DateTime? MatchDate { get; protected set; }
        public string Place { get; protected set; }
        public int? ScoreFirstTeam { get; protected set; }
        public int? ScoreSecondTeam { get; protected set; }
        public DateTime? CreatDate { get; protected set; }
        public DateTime? UpdateDate { get; protected set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public List<MatchhPlayer> Players2Match { get; set; } = new List<MatchhPlayer>();

        public Matchh()
        { }
        public Matchh(string OpponentTeam, DateTime MatchDate, string Place)
        {
            SetNameOpponentTeam(OpponentTeam);
            SetMatchDate(MatchDate);
            SetPlace(Place);
            SetCreatDate(); //dodaje date utworzenia jezeli taka juz istnieje to nic nie robi
        }
        public Matchh(string OpponentTeam, DateTime MatchDate, string Place, int ScoreFirstTeam, int ScoreSecondTeam)
        {
            SetNameOpponentTeam(OpponentTeam);
            SetMatchDate(MatchDate);
            SetPlace(Place);
            SetScoreFirstTeam(ScoreFirstTeam);
            SetScoreSecondTeam(ScoreSecondTeam);
            SetCreatDate(); //dodaje date utworzenia jezeli taka juz istnieje to nic nie robi
        }
        public void SetNameOpponentTeam(string opponentTeam)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (string.IsNullOrWhiteSpace(opponentTeam))
            {
                throw new Exception($"Match with id: '{Id}' can not have an empty opponentTeam.");
            }
            OpponentTeam = opponentTeam;
            UpdateDate = DateTime.Now;

        }
        public void SetMatchDate(DateTime matchDate)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
          //  if (matchDate<DateTime.Now) //mozna tylko dodawac mecze w przszlosci
            //{
            //    throw new Exception($"Match with id: '{Id}' can not be in the past.");
            //}
            MatchDate = matchDate;
            UpdateDate = DateTime.Now;

        }
        public void SetPlace(string place)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (string.IsNullOrWhiteSpace(place))
            {
                throw new Exception($"Match with id: '{Id}' can not have an empty place.");
            }
            Place = place;
            UpdateDate = DateTime.Now;
            //mozna w modelu dodac date modyfikacji i tu ja zmieniac
    
       
        }
        public void SetScoreFirstTeam(int scoreFirstTeam)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (scoreFirstTeam < 0)
            {
                throw new Exception($"Match with id: '{Id}' can not have an scoreFirstTeam less than zero.");
            }
            ScoreFirstTeam = scoreFirstTeam;
            UpdateDate = DateTime.Now;
            //mozna w modelu dodac date modyfikacji i tu ja zmieniac
        }
        public void SetScoreSecondTeam(int scoreSecondtTeam)  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (scoreSecondtTeam < 0)
            {
                throw new Exception($"Match with id: '{Id}' can not have an scoreSecondTeam less than zero.");
            }
            ScoreSecondTeam = scoreSecondtTeam;
            UpdateDate = DateTime.Now;
            //mozna w modelu dodac date modyfikacji i tu ja zmieniac
        }
        public void SetCreatDate()  // ta metoda zabezpiecza przez zlym uzupelnieniem przy modyfikacji pola
        {
            if (CreatDate==null)
            {
                CreatDate = DateTime.Now;
                UpdateDate = DateTime.Now;
            }
        }
    }
}
