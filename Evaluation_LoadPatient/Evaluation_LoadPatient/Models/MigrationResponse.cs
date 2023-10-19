using System.Collections.Generic;

namespace Evaluation_LoadPatient.Models
{
    internal class MigrationResponse
    {
        public bool IsSuccess { get; set; }
        public string[] Errors { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
