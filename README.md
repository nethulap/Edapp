# Edapp

To run this project locally 
```bash
1. Install .Net Core SDK (https://dotnet.microsoft.com/download)
2. Clone this project locally (git@github.com:nethulap/Edapp.git)
```
## edapp

Perform the following things to run this api on localhost:5000
```bash
1. dotnet restore
2. dotnet build
3. dotnet run
```

Once everything is build and compile you can access the api at http://localhost:8080. 
for eg: to get list of users http://localhost:8080/api/user

### Api urls

1. To get a specific user GET 
```bash
   http://localhost:5000/api/user/1
```
2. To get List of items GET 
```bash
   http://localhost:5000/api/item
```
3.  To get specific item GET
```bash
   http://localhost:5000/api/item/1
```
4. To add a new bid to an item
```bash
   http://localhost:5000/api/item/1/bid

## edapp.tests
To run tests
```bash
dotnet test
```