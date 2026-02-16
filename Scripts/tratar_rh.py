import pandas as pd
import os

# ============================
# Definir caminhos absolutos
# ============================

base_path = r"C:\Users\Lucas\Documents\Curso Power Bi\Talent Wave"

raw_path = os.path.join(base_path, "Data", "Raw", "DatasetRH.csv")
processed_path = os.path.join(base_path, "Data", "Processed")

# Criar pasta Processed caso não exista
os.makedirs(processed_path, exist_ok=True)

# ============================
# 1. Carregar dataset original
# ============================

df = pd.read_csv(raw_path)

print("\n--- Verificando duplicatas em Id_Funcionario ---")
duplicados = df[df.duplicated("Id_Funcionario", keep=False)]
print(duplicados if not duplicados.empty else "Nenhuma duplicata encontrada.")

print("\n--- Verificando valores ausentes ---")
print(df.isnull().sum())

# ==========================================
# 2. Criar coluna Promocao_Recomendada
# ==========================================

df["Promocao_Recomendada"] = df["Anos_Desde_Ultima_Promocao"].apply(
    lambda x: "Sim" if x >= 5 else "Não"
)

# ==========================================
# 3. Criar tabelas dimensão
# ==========================================

dim_genero = df[["Genero"]].drop_duplicates().reset_index(drop=True)
dim_genero["Id_Genero"] = dim_genero.index + 1

dim_estado_civil = df[["Estado Civil"]].drop_duplicates().reset_index(drop=True)
dim_estado_civil["Id_Estado_Civil"] = dim_estado_civil.index + 1

dim_departamento = df[["Departamento"]].drop_duplicates().reset_index(drop=True)
dim_departamento["Id_Departamento"] = dim_departamento.index + 1

dim_funcao = df[["Funcao"]].drop_duplicates().reset_index(drop=True)
dim_funcao["Id_Funcao"] = dim_funcao.index + 1

dim_viagem = df[["Viagem"]].drop_duplicates().reset_index(drop=True)
dim_viagem["Id_Viagem"] = dim_viagem.index + 1

# ==========================================
# 4. Mesclar IDs na tabela fato
# ==========================================

df = df.merge(dim_genero, on="Genero", how="left")
df = df.merge(dim_estado_civil, on="Estado Civil", how="left")
df = df.merge(dim_departamento, on="Departamento", how="left")
df = df.merge(dim_funcao, on="Funcao", how="left")
df = df.merge(dim_viagem, on="Viagem", how="left")

# ==========================================
# 5. Criar tabela fato final
# ==========================================

fact = df[
    [
        "Id_Funcionario",
        "Idade",
        "Valor Diaria",
        "Indice_Envolvimento_Trabalho",
        "Nivel_Satisfacao_Trabalho",
        "Salario_Mensal",
        "Numero_Empresas_Anteriores",
        "Disponivel_Hora_Extra",
        "Percentual_Ultimo_Aumento_Salario",
        "Aval_Performance",
        "Anos_Experiencia",
        "Numero_Treinamentos_Ano_Anterior",
        "Anos_na_Empresa",
        "Anos_Funcao_Atual",
        "Anos_Desde_Ultima_Promocao",
        "Anos_com_Gerente_Atual",
        "Promocao_Recomendada",
        "Id_Funcao",
        "Id_Departamento",
        "Id_Viagem",
        "Id_Genero",
        "Id_Estado_Civil",
    ]
]

# ==========================================
# 6. Exportar arquivos
# ==========================================

fact.to_csv(os.path.join(processed_path, "FactFuncionarios.csv"), index=False)
dim_genero.to_csv(os.path.join(processed_path, "DimGenero.csv"), index=False)
dim_estado_civil.to_csv(os.path.join(processed_path, "DimEstadoCivil.csv"), index=False)
dim_departamento.to_csv(os.path.join(processed_path, "DimDepartamento.csv"), index=False)
dim_funcao.to_csv(os.path.join(processed_path, "DimFuncao.csv"), index=False)
dim_viagem.to_csv(os.path.join(processed_path, "DimViagem.csv"), index=False)

print("\nArquivos gerados com sucesso em:")
print(processed_path)
