

This is a simple tool for basic Cosmos Db operations.

-----------------------------------------------------------
	lmcosmos
-----------------------------------------------------------
Usage: lmcosmos [command] [options]

Options:
  -f|--full-help  FullHelp
  -m|--markdown   Markdown
  -?|-h|--help    Show help information.

Commands:
  delete          
  patch           
  patch-many      
  select          
  settings        
  switchto        

Run 'lmcosmos [command] -?|-h|--help' for more information about a command.

-----------------------------------------------------------
	lmcosmos select
-----------------------------------------------------------
Usage: lmcosmos select [options] <ContainerName> <Query>

Arguments:
  ContainerName               
  Query                       

Options:
  -j|--json-path <JSON_PATH>  Select some fields based on a JsonPath string
  -?|-h|--help                Show help information.
Example: select persons "SELECT c.firstName, c.lastName FROM c WHERE c.lastName = 'DUPONT'"
-----------------------------------------------------------
	lmcosmos patch
-----------------------------------------------------------
Usage: lmcosmos patch [options] <ContainerName> <Id> <PatchType> <PropertyPath> <Value> <ValueType>

Arguments:
  ContainerName  
  Id             
  PatchType      
                 Allowed values are: Set, Delete, Increment.
  PropertyPath   
  Value          
  ValueType      Exemples: String, Int32, ...

Options:
  -s|--silently  Silently
  -?|-h|--help   Show help information.
Example: 
patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be set firstName "Pierre" string
patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be set age 18 Int32 
patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be increment age 3 
patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be delete teenager
-----------------------------------------------------------
	lmcosmos patch-many
-----------------------------------------------------------
Usage: lmcosmos patch-many [options] <ContainerName> <PatchType> <PropertyPath> <Value> <ValueType> <Where>

Arguments:
  ContainerName  
  PatchType      
                 Allowed values are: Set, Delete, Increment.
  PropertyPath   
  Value          
  ValueType      Exemples: String, Int32, ...
  Where          Example: "WHERE c.property = true"

Options:
  -?|-h|--help   Show help information.
Example: 
patch persons set firstName "Pierre" string "WHERE c.firstName = ''"
patch persons set age 18 Int32 "WHERE c.firstName = 'Pierre' AND c.lastName = 'DUPONT'"
patch persons increment age 3 "WHERE c.wasForgotten = true"
patch persons delete teenager "WHERE c.age = >= 18"
-----------------------------------------------------------
	lmcosmos delete
-----------------------------------------------------------
Usage: lmcosmos delete [options] <ContainerName> <Where>

Arguments:
  ContainerName  
  Where          					usage example : "WHERE c.property == 'value'"
  -m|--max-degree-of-parallelism

Options:
  -?|-h|--help   Show help information.
Example: delete persons "WHERE c.lastName = 'DUPONT'"
-----------------------------------------------------------
	lmcosmos settings
-----------------------------------------------------------
Usage: lmcosmos settings [command] [options]

Options:
  -?|-h|--help  Show help information.

Commands:
  delete        
  set           
  show          
  switchto      

Run 'settings [command] -?|-h|--help' for more information about a command.

-----------------------------------------------------------
	lmcosmos settings show
-----------------------------------------------------------
Usage: lmcosmos settings show [options]

Options:
  -?|-h|--help  Show help information.

-----------------------------------------------------------
	lmcosmos settings switchto
-----------------------------------------------------------
Usage: lmcosmos settings switchto [options] <Environment>

Arguments:
  Environment   

Options:
  -?|-h|--help  Show help information.

-----------------------------------------------------------
	set
-----------------------------------------------------------
Usage: lmcosmos settings set [command] [options]

Options:
  -?|-h|--help       Show help information.

Commands:
  connection-string  
  database           

Run 'set [command] -?|-h|--help' for more information about a command.

-----------------------------------------------------------
	lmcosmos settings set connection-string
-----------------------------------------------------------
Usage: lmcosmos settings set connection-string [options] <ConnectionString> <Environment>

Arguments:
  ConnectionString  
  Environment

Options:
  -?|-h|--help      Show help information.

-----------------------------------------------------------
	lmcosmos settings set database
-----------------------------------------------------------
Usage: lmcosmos settings set database [options] <Database> <Environment>

Arguments:
  Database      
  Environment   

Options:
  -?|-h|--help  Show help information.

-----------------------------------------------------------
	lmcosmos settings delete
-----------------------------------------------------------
Usage: lmcosmos settings delete [options] <Environment>

Arguments:
  Environment   

Options:
  -?|-h|--help  Show help information.

