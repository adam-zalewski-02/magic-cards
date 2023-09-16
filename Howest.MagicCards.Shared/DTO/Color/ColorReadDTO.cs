using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.Color
{
    public record ColorReadDTO
    {
        public string Code { get; init; }
        public string Name { get; init; }
    }
}
