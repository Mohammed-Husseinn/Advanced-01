#region Advanced Questions

//Question1: What is a generic class? Why use generics?

//A generic class defines a type parameter that is supplied when the class is used.
//Generics provide:
//Type safety at compile time
//Code reuse for different data types
//Fewer casts
//Better performance for value types because they avoid unnecessary boxing

using System.Data;

public class Box<T>
{
    public T Value { get; set; } = default!;
}

Box<int> numberBox = new() { Value = 10 };
Box<string> textBox = new() { Value = "Hello" };


//Question2:Generic Container<T> with Add and Get

public class Container<T>
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T Get(int index)
    {
        return _items[index];
    }
}

//usage//
var numbers = new Container<int>();
numbers.Add(10);
int value = numbers.Get(0);


//Question3:What are multiple type parameters? Write Pair<TKey, TValue>
//Multiple type parameters allow a generic type to work with more than one type.
public class Pair<TKey, TValue>
{
    public TKey Key { get; }
    public TValue Value { get; }

    public Pair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}

//usage//
var pair = new Pair<int, string>(1, "One");


//Question4: 
//A generic method defines its own type parameter independently of the containing class.

public static class Utility
{
    public static void Swap<T>(ref T first, ref T second)
    {
        (first, second) = (second, first);
    }
}

//usage//
int first = 10;
int second = 20;

Utility.Swap(ref first, ref second);


//Question5: Generic FindMax<T> method
//T must support comparison, so it uses the IComparable<T> constraint.
public static class EnumerableUtilities
{
    public static T FindMax<T>(IEnumerable<T> values)
        where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(values);

        using IEnumerator<T> enumerator = values.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException(
                "The sequence must contain at least one element.");
        }

        T maximum = enumerator.Current;

        while (enumerator.MoveNext())
        {
            if (enumerator.Current.CompareTo(maximum) > 0)
            {
                maximum = enumerator.Current;
            }
        }

        return maximum;
    }
}

//usage//
int maximum = EnumerableUtilities.FindMax(new[] { 3, 8, 2, 5 });

//Question6:  What is a generic interface? Write IRepository<T>
//A generic interface defines operations that work with a specified type.
public interface IRepository<T>
{
    void Add(T entity);

    bool TryGetById(int id, out T entity);

    void Remove(int id);

    IEnumerable<T> GetAll();
}
//The interface can be implemented for different entity types:
public class User
{
    public int Id { get; set; }
}

public class UserRepository : IRepository<User>
{
    private readonly List<User> _users = new();

    public void Add(User entity)
    {
        _users.Add(entity);
    }

    public bool TryGetById(int id, out User entity)
    {
        entity = _users.Find(user => user.Id == id)!;
        return entity is not null;
    }

    public void Remove(int id)
    {
        _users.RemoveAll(user => user.Id == id);
    }

    public IEnumerable<User> GetAll()
    {
        return _users;
    }
}

//Question7:What is the struct constraint?
//The struct constraint requires T to be a non-nullable value type.
public static class ValueTypeUtility
{
    public static bool IsDefault<T>(T value)
        where T : struct
    {
        return EqualityComparer<T>.Default.Equals(value, default);
    }
}

//Valid types include int, double, DateTime, and custom structs.

//Q8: What is the class constraint?
//The class constraint requires T to be a reference type.
public static class ReferenceTypeUtility
{
    public static string Describe<T>(T value)
        where T : class
    {
        return value?.ToString() ?? "(null)";
    }
}

//Valid types include string, arrays, classes, and interfaces.

//Q9: What is the new() constraint ?
//The new() constraint requires T to have a public parameterless constructor.
public static class Factory
{
    public static T Create<T>()
        where T : new()
    {
        return new T();
    }
}

//Example//
public class Customer
{
    public Customer()
    {
    }
}

Customer customer = Factory.Create<Customer>();

//Q10: What is an interface constraint?
//An interface constraint requires T to implement a specified interface.
public interface IHasId
{
    int Id { get; }
}

public static class EntityUtility
{
    public static int GetId<T>(T entity)
        where T : IHasId
    {
        return entity.Id;
    }
}

//Q11: What is a base class constraint?
//A base class constraint requires T to inherit from a specified class.
public abstract class Animal
{
    public string Name { get; set; } = string.Empty;
}

public class AnimalShelter<T>
    where T : Animal
{
    public void Admit(T animal)
    {
        Console.WriteLine($"Admitted: {animal.Name}");
    }
}
//AnimalShelter<Dog> is valid if Dog inherits from Animal.

//Q12: How do you apply multiple constraints?
//Multiple constraints are separated by commas.
public abstract class Entity
{
    public int Id { get; set; }
}

public interface IValidatable
{
    bool IsValid();
}

public class EntityFactory<T>
    where T : Entity, IValidatable, new()
{
    public T CreateValidEntity()
    {
        T entity = new();

        if (!entity.IsValid())
        {
            throw new InvalidOperationException("The entity is invalid.");
        }

        return entity;
    }
}

//The usual order is:
//1.  class or struct
//2.Base class
//3.Interfaces
//4.  new()

//Q13: What does the default keyword do in generics?
//default returns the default value for a type:
//0 for numeric value types
//false for bool
//A zero - initialized struct
//null for reference types

public static T GetDefault<T>()
{
    return default!;
}
//example//
int number = default;       // 0
bool flag = default;        // false
string? text = default;     // null

//Q14: SafeList<T> returning default for an invalid index
public class SafeList<T>
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T Get(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return default!;
        }

        return _items[index];
    }
}
//For a reference type, an invalid index returns null. For a value type, it returns its default value.
//Q15: What is covariance ? Explain out
//Covariance allows a generic type containing a derived type to be assigned to a generic type containing its base type.
//The out keyword is used for type parameters that are produced or returned.
public interface IProducer<out T>
{
    T Produce();
}
//Example//
IProducer<string> stringProducer = default!;
IProducer<object> objectProducer = stringProducer;
//A producer of string can be treated as a producer of object because every string is an object.

//Q16: What is contravariance? Explain in
//Contravariance allows a generic type containing a base type to be assigned to a generic type containing a derived type.
//The in keyword is used for type parameters that are consumed as method parameters.

public interface IConsumer<in T>
{
    void Consume(T value);
}

//example//
IConsumer<object> objectConsumer = default!;
IConsumer<string> stringConsumer = objectConsumer;
//A consumer that accepts any object can also consume strings.

//Q17: Difference between covariance and contravariance
//Feature	Keyword	Direction	Typical use
//Covariance	out	Derived to base	Producers and readers
//Contravariance	in	Base to derived	Consumers and writers
//Invariance	None	No implicit conversion	Types that both read and write


//example//

IProducer<string> producer = default!;
IProducer<object> covariantProducer = producer;
IConsumer<object> consumer = default!;
IConsumer<string> contravariantConsumer = consumer;

//Q18: How do static members work in generic types?
//Each closed generic type gets its own static members

public static class TypeCounter<T>
{
    public static int Count { get; set; }
}

//usage//
TypeCounter<int>.Count = 10;
TypeCounter<string>.Count = 20;

Console.WriteLine(TypeCounter<int>.Count);    // 10
Console.WriteLine(TypeCounter<string>.Count); // 20
//TypeCounter<int>.Count = 10;
TypeCounter<string>.Count = 20;

//TypeCounter<int> and TypeCounter<string> have separate static storage.



//Q19: How can you inherit from a generic class?
//A derived class can specify a concrete type:
public class StringContainer : Container<string>
{
}
//Or the derived class can remain generic:
public class SpecializedContainer<T> : Container<T>
{
}


//usage//
var strings = new StringContainer();
strings.Add("Hello");

var numbers = new SpecializedContainer<int>();
numbers.Add(100);



//Q20: Generic cache with expiration support
//This implementation:
//	Supports Add, Get, Remove, and Contains
//	Allows a default expiration
//	Allows an expiration per item
//	Replaces an existing value when the same key is added
//	Removes expired items when they are accessed
//	Is thread-safe
//	Throws KeyNotFoundException when Get cannot find a valid item
public sealed class Cache<TKey, TValue>
    where TKey : notnull
{
    private readonly object _syncRoot = new();
    private readonly Dictionary<TKey, CacheEntry> _entries = new();
    private readonly TimeSpan? _defaultExpiration;

    public Cache(TimeSpan? defaultExpiration = null)
    {
        ValidateExpiration(defaultExpiration, nameof(defaultExpiration));
        _defaultExpiration = defaultExpiration;
    }

    public void Add(
        TKey key,
        TValue value,
        TimeSpan? expiration = null)
    {
        ValidateExpiration(expiration, nameof(expiration));

        TimeSpan? duration = expiration ?? _defaultExpiration;

        DateTimeOffset? expiresAt = duration.HasValue
            ? DateTimeOffset.UtcNow.Add(duration.Value)
            : null;

        lock (_syncRoot)
        {
            _entries[key] = new CacheEntry(value, expiresAt);
        }
    }

    public TValue Get(TKey key)
    {
        lock (_syncRoot)
        {
            if (!_entries.TryGetValue(key, out CacheEntry entry))
            {
                throw new KeyNotFoundException(
                    "The specified key was not found.");
            }

            if (IsExpired(entry))
            {
                _entries.Remove(key);

                throw new KeyNotFoundException(
                    "The specified key has expired.");
            }

            return entry.Value;
        }
    }

    public bool Contains(TKey key)
    {
        lock (_syncRoot)
        {
            if (!_entries.TryGetValue(key, out CacheEntry entry))
            {
                return false;
            }

            if (IsExpired(entry))
            {
                _entries.Remove(key);
                return false;
            }

            return true;
        }
    }

    public bool Remove(TKey key)
    {
        lock (_syncRoot)
        {
            return _entries.Remove(key);
        }
    }

    private static bool IsExpired(CacheEntry entry)
    {
        return entry.ExpiresAt.HasValue &&
               entry.ExpiresAt.Value <= DateTimeOffset.UtcNow;
    }

    private static void ValidateExpiration(
        TimeSpan? expiration,
        string parameterName)
    {
        if (expiration.HasValue && expiration.Value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                "Expiration must be greater than zero.");
        }
    }

    private readonly record struct CacheEntry(
        TValue Value,
        DateTimeOffset? ExpiresAt);
}


//usage//
var cache = new Cache<string, int>(
    TimeSpan.FromMinutes(5));

cache.Add("answer", 42);

if (cache.Contains("answer"))
{
    int value = cache.Get("answer");
}

cache.Add(
    "temporary",
    100,
    TimeSpan.FromSeconds(30));

cache.Remove("answer");

//Expiration is checked lazily when Get or Contains is called. No background cleanup thread is required.








#endregion