# ADO Wrapper (Beta)

![](https://i.ibb.co/3hpcnxX/ADO-NET.jpg)

### Features

- Supports Default ASP Net Core dependency Injection.
- Fast and lightweight.
- Easy to use.
- uses ADO Net. 
- Supports Navigation Properties (up to one level at the moment)

### Updates
- 0.2.0 : A Bug in ```DBNULL``` values has been fixed. Null Values are now assigned to their default values
- 0.3.0 : Navigation Properties are now optional. ```UsingNavigation``` flag added to ```GetList``` and ```GetListAsync``` methods. default is ```true```. Set it to false wherever you need

### Installation
Using Package Manager Console:

`Install-Package AdoWrapper -Version 0.1.0`

Using NET CLI:

`dotnet add package AdoWrapper --version 0.1.0`

### Setting Up

If you are using good old Startup.cs, add The following code to Startup.cs class in ConfigureServices method:

```Csharp
 services.AddAdoWrapper("Your Connection String");
 ```
if your are using new feature of NET 6.0 add the following code to Program.cs class

```Csharp
builder.Services.AddAdoWrapper("Your Connection String");
```
this adds the required services for ADO warpper to work.

Now you can inject `IAdoProvider` interface where ever you need. Like

```Csharp
public class BookController : ControllerBase
    {
        private readonly IAdoProvider _ado;

        public BookController(IAdoProvider ado)
        {
            _ado = ado;
        }
	}
```

### Methods

ADO wrapper has two main methods:

 ```Csharp
 GetList<TClass>("SQL query");
 GetFirstOrDefault<TClass>("SQL Query");
 ```
Both of these methods have Async equivalents.

` GetList<TClass>` returns list of data type `TClass` where `GetFirstOrDefault<TClass>` returns single instance of the class. Keep in mind that `TClass` object should have a default parameterless constructor and for using the ` GetList<TClass>` the class should implement the `IEquatable<T>` interface.

**Querying Data**

Consider the following class

```Csharp
 public class SimpleAuthor:IEquatable<SimpleAuthor>
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }
        public bool Equals(SimpleAuthor? other)
        {
            if (other is null)
                return false;

            return this.ID == other.ID;
        }
    }

```
for querying list of data you can use ADO Wrapper like this. Keep in mind that the name of properties in the given Class should be as same as the name of them in their corresponding SQL table:

```Csharp
var result = 
                _ado.GetList<SimpleAuthor>(
                    "Select * from Authors");
```

the result is somthing like this:

```json
[
  {
    "id": 1,
    "authorName": "Babak"
  },
  {
    "id": 2,
    "authorName": "Ali"
  }
]
```

or for a single data you can use it like this:

```Csharp
var result = _ado.GetFirstOrDefault<SimpleAuthor>("Select top(1) * from Authors Order By Authors.ID desc");
```
the result would look something just like this:

```json
{
  "id": 2,
  "authorName": "Ali"
}
```
### Using navigation properties

ADO wrapper works best when you intend to use it for flat and denormalized data. however you can have navigation properties as well (up to one level at the moment).
There are two marker attributes that you should use in your classes called `[ForeignKeyNavigation]` and `[ForeignNavigation]` .

The `[ForeignNavigation]` attribute declares a class or list to be the foriegn navigation and the `[ForeignKeyNavigation]` declares the property of a child class to be the foreign key of the parent class and it takes the name of the primary key property of the parent class as its constructor. Consider the following two classes:

```csharp
 public class Author:IEquatable<Author>
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }

        [ForeignNavigation]
        public List<Book> Books { get; set; }

        public bool Equals(Author? other)
        {
            if (other is null)
                return false;

            return this.ID == other.ID;
        }
    }
```
the `Book` class is the child entity of the parent class `Author`. It looks like this:

```csharp
 public class Book
    {
        public int Id { get; set; }

        [ForeignKeyNavigation(nameof(Author.ID))]
        public int AuthorId { get; set; }
        public string Name { get; set; }
    }
```
now you can use ADO wrapper for querying data like this:

```csharp
var result =  _ado.GetList<Author>("select * from Authors Left join Books on Authors.ID=Books.AuthorId");
```
the result would look something like this:

```json
[
  {
    "id": 1,
    "authorName": "Babak",
    "books": [
      {
        "id": 1,
        "authorId": 1,
        "name": "Babak's First Book"
      },
      {
        "id": 2,
        "authorId": 1,
        "name": "Babak's Second Book"
      },
      {
        "id": 4,
        "authorId": 1,
        "name": "Babak's Third Book"
      }
    ]
  },
  {
    "id": 2,
    "authorName": "Ali",
    "books": [
      {
        "id": 3,
        "authorId": 2,
        "name": "Ali's First Book"
      },
      {
        "id": 5,
        "authorId": 2,
        "name": "Ali's Second Book"
      }
    ]
  }
]
```

For single data you can do so like this:

```csharp
var result =
                _ado.GetFirstOrDefault<SingleAuthor>(
                    "select * from Authors Left join Books on Authors.ID=Books.AuthorId where Authors.ID=1");
```
and the result would look something like this:
```json
{
  "id": 1,
  "authorName": "Babak",
  "books": [
    {
      "id": 1,
      "authorId": 1,
      "name": "Babak's First Book"
    },
    {
      "id": 2,
      "authorId": 1,
      "name": "Babak's Second Book"
    },
    {
      "id": 4,
      "authorId": 1,
      "name": "Babak's Third Book"
    }
  ]
}
```

You can use a simple class rather than list as navigation property as well. Consider the following classes:

```csharp
  public class SingleAuthorSingleNavigation
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }

        [ForeignNavigation]
        public Book SingleBook { get; set; }
    }
```

```csharp
 public class Book
    {
        public int Id { get; set; }

        [ForeignKeyNavigation(nameof(Author.ID))]
        public int AuthorId { get; set; }
        public string Name { get; set; }
    }
```
now you can query data like this using ADO wrapper:

```csharp
 var result =
                _ado.GetFirstOrDefault<SingleAuthorSingleNavigation>(
                    "select * from Authors Left join Books on Authors.ID = Books.AuthorId where Authors.ID = 1 and Books.ID = 4");
```
the result would look something like this:

```json
{
  "id": 1,
  "authorName": "Babak",
  "singleBook": {
    "id": 4,
    "authorId": 1,
    "name": "Babak's Third Book"
  }
}
```
keep in mind that you don't need to specify a property as foreign key using  `[ForeignKeyNavigation]` attribute in this situation. But it's a good practice to do so.

### WARNING ⚠️
Currently ADO wrapper does not provide you protection against SQL injection attacks. So I recommend you not to use string interpolation as SQL query and parametrize your parameters before hand.

### your help is greatly appreciated !
Currently ADO wrapper is in early stages. I have plans to make it better and do some optimizations on it. You can help me by creating PRs or creating issues on this repository and I'll check them as soon as I can!
If you like this library you can give it a star as well.
