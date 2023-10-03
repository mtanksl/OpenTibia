server = {
	login = {
		port = 7171
	},
	game = {
		port = 7172,
		maxplayers = 1000
	},		
	database = {
		type = "mysql",

		-- sqlite
		source = "data/database.db",

		-- mysql or mssql
		host = "localhost",
		port = 3306,
		user = "root",
		password = "123456",
		name = "mtots",
	}
}