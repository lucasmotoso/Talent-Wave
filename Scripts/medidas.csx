// =========================
// REFERÊNCIAS
// =========================
var tabelaMedidas = Model.Tables["Medidas"];
var ms = tabelaMedidas.Measures;

// =========================
// Helper: aplica DisplayFolder
// =========================
Action<string, string> SetFolder = (measureName, folder) =>
{
    if (ms.Contains(measureName))
        ms[measureName].DisplayFolder = folder;
};

// =========================
// 01 – KPIs Gerais
// =========================
SetFolder("Total Funcionários", "01 – KPIs Gerais");
SetFolder("Experiência Média (Anos)", "01 – KPIs Gerais");
SetFolder("Salário Médio Mensal", "01 – KPIs Gerais");

// =========================
// 02 – Gênero
// =========================
SetFolder("Total Masculino", "02 – Gênero");
SetFolder("Total Feminino", "02 – Gênero");
SetFolder("% Masculino", "02 – Gênero");
SetFolder("% Feminino", "02 – Gênero");

// =========================
// 03 – Experiência
// =========================
SetFolder("Experiência Média (Anos)", "03 – Experiência");

// =========================
// 04 – Salário
// =========================
SetFolder("Salário Médio Mensal", "04 – Salário");

// =========================
// 05 – Função
// =========================
SetFolder("Total Funcionários por Função", "05 – Função");

// =========================
// 06 – Hora Extra
// =========================
SetFolder("Total Disponíveis Hora Extra", "06 – Hora Extra");
SetFolder("% Disponíveis Hora Extra", "06 – Hora Extra");

// =========================
// 07 – Envolvimento
// =========================
SetFolder("Total Envolvimento Ruim", "07 – Envolvimento");
SetFolder("Total Envolvimento Baixo", "07 – Envolvimento");
SetFolder("Total Envolvimento Médio", "07 – Envolvimento");
SetFolder("Total Envolvimento Alto", "07 – Envolvimento");

// =========================
// 08 – Promoção
// =========================
SetFolder("Total Funcionários a Promover", "08 – Promoção");
SetFolder("% Funcionários a Promover", "08 – Promoção");

// =========================
// Helper: cria ou atualiza medida
// =========================
Func<string, string, string, string, object> UpsertMeasure =
    (name, dax, description, folder) =>
{
    var m = ms.Contains(name)
        ? ms[name]
        : tabelaMedidas.AddMeasure(name, dax, description);

    m.Expression = dax;
    m.Description = description;
    m.DisplayFolder = folder;
    return m;
};

// =========================
// PROMOÇÃO – REGRA DE NEGÓCIO
// =========================
var aptos = UpsertMeasure(
    "Funcionários Aptos à Promoção",
    "CALCULATE( [Total Funcionários], 'FactFuncionarios'[Anos_Desde_Ultima_Promocao] >= 5 )",
    "Funcionários com 5 anos ou mais desde a última promoção",
    "08 – Promoção"
);
((dynamic)aptos).FormatString = "#,0";

var naoAptos = UpsertMeasure(
    "Funcionários Não Aptos à Promoção",
    "CALCULATE( [Total Funcionários], 'FactFuncionarios'[Anos_Desde_Ultima_Promocao] < 5 )",
    "Funcionários com menos de 5 anos desde a última promoção",
    "08 – Promoção"
);
((dynamic)naoAptos).FormatString = "#,0";
// =========================
// =========================
// 09 – PERFORMANCE
// =========================

// % Funcionários Aptos à Promoção
var pctAptos = UpsertMeasure(
    "% Funcionários Aptos à Promoção",
    "DIVIDE( [Funcionários Aptos à Promoção], [Total Funcionários] )",
    "Percentual de funcionários aptos à promoção",
    "09 – Performance"
);
((dynamic)pctAptos).FormatString = "0.00%";

// Média de Performance (coluna numérica)
var mediaPerformance = UpsertMeasure(
    "Média de Performance",
    "AVERAGE( 'FactFuncionarios'[Aval_Performance] )",
    "Média de performance dos funcionários (escala 1 a 4)",
    "09 – Performance"
);
((dynamic)mediaPerformance).FormatString = "0.00";

// % Engajamento Alto
var pctEngAlto = UpsertMeasure(
    "% Engajamento Alto",
    "DIVIDE( [Total Envolvimento Alto], [Total Funcionários] )",
    "Percentual de funcionários com engajamento alto",
    "09 – Performance"
);
((dynamic)pctEngAlto).FormatString = "0.00%";

// % Engajamento Ruim
var pctEngRuim = UpsertMeasure(
    "% Engajamento Ruim",
    "DIVIDE( [Total Envolvimento Ruim], [Total Funcionários] )",
    "Percentual de funcionários com engajamento ruim",
    "09 – Performance"
);
((dynamic)pctEngRuim).FormatString = "0.00%";
// =========================
// 09 – PERFORMANCE (RISCO)
// =========================

// Funcionários com Performance Crítica (Ruim + Baixo)
var perfCritica = UpsertMeasure(
    "Funcionários com Performance Crítica",
    "CALCULATE( [Total Funcionários], 'FactFuncionarios'[Nivel_Satisfacao_Trabalho] <= 2 )",
    "Funcionários com performance ruim ou baixa (nível de risco)",
    "09 – Performance (Risco)"
);
((dynamic)perfCritica).FormatString = "#,0";

// Funcionários com Performance Saudável (Médio + Alto)
var perfSaudavel = UpsertMeasure(
    "Funcionários com Performance Saudável",
    "CALCULATE( [Total Funcionários], 'FactFuncionarios'[Nivel_Satisfacao_Trabalho] >= 3 )",
    "Funcionários com performance média ou alta (nível saudável)",
    "09 – Performance (Risco)"
);
((dynamic)perfSaudavel).FormatString = "#,0";

// % Performance Crítica
var pctPerfCritica = UpsertMeasure(
    "% Performance Crítica",
    "DIVIDE( [Funcionários com Performance Crítica], [Total Funcionários] )",
    "Percentual de funcionários em risco de performance",
    "09 – Performance (Risco)"
);
((dynamic)pctPerfCritica).FormatString = "0.00%";

// % Performance Saudável
var pctPerfSaudavel = UpsertMeasure(
    "% Performance Saudável",
    "DIVIDE( [Funcionários com Performance Saudável], [Total Funcionários] )",
    "Percentual de funcionários com performance saudável",
    "09 – Performance (Risco)"
);
((dynamic)pctPerfSaudavel).FormatString = "0.00%";
