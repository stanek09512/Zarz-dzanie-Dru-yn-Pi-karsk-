using AutoMapper;
using InzBackCore.Domain;
using InzBackInfrastructure.Commands.Matches;
using InzBackInfrastructure.Commands.Players;
using InzBackInfrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace InzBackInfrastructure.Mappers
{
    public static class  AutoMapperConfig
    {
        public static IMapper Initialize()  //metoda zwraca implementacje interfejsu IMapper
            => new MapperConfiguration(cfg =>   //zwroc nowa konfiguracje mappera
            {
                cfg.CreateMap<Player, PlayerDto>(); //stworz mape z player na player dto
                cfg.CreateMap<GetPlayersStatisticInMatchDto, MatchhPlayer>();
                cfg.CreateMap<MatchhPlayer, GetPlayersStatisticInMatchDto>();
               // cfg.CreateMap<PlayersStatictics, GetPlayersStatsList>();
                // cfg.CreateMap<Player, PutPlayer>();
                //forMember(x=>x.pole, m=>m.MapFrom(p=>p.pole.count) przyklad mapowanie na jakies pola wyliczeniowe np bazujace na innym obiekcie
            })
            .CreateMapper();    //utworz implementacje naszego interfejsu
    }
}
