Feature: Product API
	Covers basic API Requests to test the Product API

	@smoke
	Scenario: GET a single product
		When user sends a request to GET a single product to endpoint "Product/GetProductById/{id}"
			| ProductId |
			| 1         |
		Then response body contains product name "Keyboard"


	@smoke
	Scenario Outline: GET products by ID
		When user sends a GET request to get product with id '<ProductId>' to endpoint "Product/GetProductById/{id}"
		Then response body contains product with name '<Name>'

		Examples:
			| ProductId | Name     |
			| 1         | Keyboard |
			| 2         | Monitor  |
			| 3         | Mouse    |


	@smoke
	Scenario: GET a product by ID and Name
		When user sends a GET request with '<ProductId>' and '<Name>' to endpoint "/Product/GetProductByIdAndName"
		Then response body contains product name '<ProductId>' and '<Name>'

		Examples:
			| ProductId | Name     |
			| 1         | Keyboard |
			| 2         | Monitor  |
			| 3         | Mouse    |


	@smoke
	Scenario: GET all products
		When user sends a request to GET all products to endpoint "/Product/GetProducts"
		Then response status code is OK


	@smoke
	Scenario: POST Create a new product
		When user sends a POST request to create a new product to endpoint "/Product/Create"
			| Name    | Description    | Price |
			| Printer | Colour printer | 300   |

		Then response body contains 
			| Name    | Description    | Price |
			| Printer | Colour printer | 300   |
