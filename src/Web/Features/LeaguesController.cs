﻿namespace BettingSystem.Web.Features
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Common;
    using Application.Competitions.Leagues.Commands.Create;
    using Application.Competitions.Leagues.Commands.Edit;
    using Application.Competitions.Leagues.Queries.Standings;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AuthorizeAdministrator]
    public class LeaguesController : ApiController
    {
        [HttpGet(Id)]
        [AllowAnonymous]
        [Route(Id + PathSeparator + nameof(Standings))]
        public async Task<ActionResult<IEnumerable<LeagueStandingsResponseModel>>> Standings(
            [FromRoute] LeagueStandingsQuery query)
            => await this.Send(query);

        [HttpPost]
        public async Task<ActionResult<CreateLeagueResponseModel>> Create(
            CreateLeagueCommand command)
            => await this.Send(command);

        [HttpPut]
        [Route(Id)]
        public async Task<ActionResult> Edit(
            int id, EditLeagueCommand command)
            => await this.Send(command.SetId(id));
    }
}