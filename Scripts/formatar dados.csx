var medidas = Model.Tables["Medidas"].Measures;

// =========================
// FORMATO: INTEIRO
// =========================
string[] inteiros = {
    "Total Funcionários",
    "Total Masculino",
    "Total Feminino",
    "Total Funcionários por Função",
    "Total Disponíveis Hora Extra",
    "Total Envolvimento Ruim",
    "Total Envolvimento Baixo",
    "Total Envolvimento Médio",
    "Total Envolvimento Alto",
    "Total Funcionários a Promover"
};

foreach (var nome in inteiros)
{
    if (medidas.Contains(nome))
    {
        medidas[nome].FormatString = "#,0";
    }
}

// =========================
// FORMATO: MOEDA (R$)
// =========================
string[] moedas = {
    "Salário Médio Mensal"
};

foreach (var nome in moedas)
{
    if (medidas.Contains(nome))
    {
        medidas[nome].FormatString = "R$ #,0.00";
    }
}

// =========================
// FORMATO: PORCENTAGEM
// =========================
string[] porcentagens = {
    "% Masculino",
    "% Feminino",
    "% Disponíveis Hora Extra",
    "% Funcionários a Promover"
};

foreach (var nome in porcentagens)
{
    if (medidas.Contains(nome))
    {
        medidas[nome].FormatString = "0.00%";
    }
}

// =========================
// FORMATO: DECIMAL (1 casa)
// =========================
string[] decimais = {
    "Experiência Média (Anos)"
};

foreach (var nome in decimais)
{
    if (medidas.Contains(nome))
    {
        medidas[nome].FormatString = "#,0.0";
    }
}
