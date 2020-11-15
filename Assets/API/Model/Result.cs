using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
 public partial class Result
    {
        [JsonProperty("sucesso")]
        public bool Sucesso { get; set; }

        [JsonProperty("retorno")]
        public Escola Escola { get; set; }
    }

    public partial class Escola
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("turmas")]
        public Turma[] Turmas { get; set; }
    }

    public partial class Turma
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }

