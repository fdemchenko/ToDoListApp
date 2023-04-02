CREATE TABLE Categories (
	categoryId SERIAL PRIMARY KEY,
	name VARCHAR(50) NOT NULL
);

CREATE TABLE ToDoItems (
	todoitemId SERIAL PRIMARY KEY,
	name VARCHAR(100) NOT NULL,
	dueDate TIMESTAMP,
	categoryId INT NOT NULL,
	CONSTRAINT fkCategory FOREIGN KEY(categoryId)
		REFERENCES Categories(categoryId)
);