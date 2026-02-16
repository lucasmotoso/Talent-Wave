library(ggplot2)
library(dplyr)
library(scales)

bg <- alpha("#324E55", 0.7)  # 30% transparente

# Preparação dos dados (sem N/A)
df <- dataset %>%
  filter(!is.na(Anos_Experiencia), !is.na(Nivel_Satisfacao_Trabalho)) %>%
  mutate(
    Experiencia_Faixa = cut(
      Anos_Experiencia,
      breaks = c(0, 5, 10, 15, 20, Inf),
      labels = c("0–5", "6–10", "11–15", "16–20", "20+"),
      include.lowest = TRUE
    ),
    Satisfacao_Faixa = cut(
      Nivel_Satisfacao_Trabalho,
      breaks = c(0, 2, 3, 4, 5),
      labels = c("Baixa", "Média", "Alta", "Muito Alta"),
      include.lowest = TRUE
    )
  ) %>%
  filter(!is.na(Experiencia_Faixa), !is.na(Satisfacao_Faixa)) %>%
  mutate(
    Experiencia_Faixa = factor(Experiencia_Faixa, levels = c("0–5","6–10","11–15","16–20","20+")),
    Satisfacao_Faixa  = factor(Satisfacao_Faixa,  levels = c("Baixa","Média","Alta","Muito Alta"))
  ) %>%
  count(Experiencia_Faixa, Satisfacao_Faixa, name = "n")

# Se não houver dados após os cortes, ainda assim gera um visual (não quebra)
if (nrow(df) == 0) {
  p <- ggplot() +
    annotate(
      "text", x = 0, y = 0,
      label = "Sem dados para exibir\n(verifique filtros/faixas)",
      color = "#FFFFFF", family = "Verdana", size = 12
    ) +
    theme_void(base_family = "Verdana", base_size = 24) +
    theme(
      plot.background  = element_rect(fill = bg, color = NA),
      panel.background = element_rect(fill = bg, color = NA)
    )
  print(p)
} else {
  p <- ggplot(df, aes(x = Experiencia_Faixa, y = Satisfacao_Faixa, fill = n)) +
    geom_tile(color = "#324E55", linewidth = 1.2) +
    geom_text(aes(label = n), color = "#FFFFFF", family = "Verdana", size = 12) +
    scale_fill_gradientn(colors = c("#0B1320", "#1F9E89", "#F2C94C", "#E63946")) +
    labs(
      title = "Distribuição de Colaboradores por Experiência e Satisfação",
      x = "Faixa de Experiência (anos)",
      y = "Nível de Satisfação"
    ) +
    theme_minimal(base_family = "Verdana", base_size = 24) +
    theme(
      plot.background  = element_rect(fill = bg, color = NA),
      panel.background = element_rect(fill = bg, color = NA),
      panel.grid = element_blank(),
      axis.text.x = element_text(color = "#FFFFFF", size = 26, family = "Verdana", face = "bold"),
      axis.text.y = element_text(color = "#FFFFFF", size = 26, family = "Verdana", face = "bold"),
      axis.title  = element_text(color = "#FFFFFF", face = "bold"),
      plot.title  = element_text(color = "#FFFFFF", face = "bold", hjust = 0.5),
      legend.position = "none"
    )

  print(p)
}
