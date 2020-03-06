Execute a aplicação pelo terminal para conseguir passar parametros.

Exemplo:
	"C:\caminho da app.exe" full


Lista de parametros e suas ações:
	"C:\caminho da app.exe" atlz
			Exporta somente os registros que foram atualizados.
			
	"C:\caminho da app.exe" regs
			Exporta todos os registros das bases (que são exportáveis).
			
	"C:\caminho da app.exe" files
			Exporta os arquivos das Normas e dos DODFs (PDF e HTML).
			
	"C:\caminho da app.exe" mapping
			Exporta Somente as configurações de mapping (das bases que possuem).
			
	"C:\caminho da app.exe" delete
			Compara os registros do LBW com os do ES e deleta do ES os que não existem no LBW.
			
	"C:\caminho da app.exe" full
			Exporta todos os registros das bases e também os arquivos das Normas e dos DODFs.
			
	"C:\caminho da app.exe" full mapping
			Exporta as configurações de mapping e exporta todos os registros das bases e também os arquivos das Normas e dos DODFs.
			
	"C:\caminho da app.exe" regs mapping
			Exporta as configurações de mapping e exporta todos os registros das bases e também os arquivos das Normas e dos DODFs.
			
	"C:\caminho da app.exe" files mapping
			Exporta as configurações de mapping e exporta todos os registros das bases e também os arquivos das Normas e dos DODFs.


Caso execute a aplicação sem passar os parametros, ela irá solicitar que informe a Base que quer exportar, uma query para recuperar os registros e se deseja exportar arquivos da base informada.