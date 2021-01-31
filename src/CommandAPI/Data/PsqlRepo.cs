using System.Collections.Generic;
using CommandAPI.Dto;
using CommandAPI.Models;
using System.Linq;
using System;

namespace CommandAPI.Data
{
    public class PsqlRepo : ICommandAPIRepo
    {
        private readonly CommandContext _ctx;
        public PsqlRepo(CommandContext ctx)
        {
            _ctx = ctx;
        }
        void ICommandAPIRepo.CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException();
            }
            _ctx.CommandItems.Add(cmd);
        }

        void ICommandAPIRepo.DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException();
            }
            _ctx.CommandItems.Remove(cmd);
        }

        IEnumerable<Command> ICommandAPIRepo.GetAllCommands() => _ctx.CommandItems.ToList();

        Command ICommandAPIRepo.GetCommandById(int id) => _ctx.CommandItems.Find(id);

        bool ICommandAPIRepo.SaveChanges() => _ctx.SaveChanges() >= 0;

        void ICommandAPIRepo.UpdateCommand(Command cmd)
        {
        }
    }
}