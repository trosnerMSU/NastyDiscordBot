﻿using Discord;
using Discord.Commands;

namespace NastyDiscordBot.TypeReaders {
    public class BooleanTypeReader : TypeReader {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services) {
            bool result;
            if(bool.TryParse(input, out result)) {
                return Task.FromResult(TypeReaderResult.FromSuccess(result));
            }

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a boolean value."));
        }

    }
}
