using System.Collections;

namespace Caliberweb.Core.Caching
{
    public interface ILocalData
    {
        ///<summary>Returns an enumerator that iterates through the collection.</summary>
        ///<returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Retrieves an object from the collection.
        /// </summary>
        /// <typeparam name="T">The type of object that is expected to be found stored in the collection.</typeparam>
        /// <param name="key">The key that the object is stored under.</param>
        /// <returns>If it exists, the stored object; otherwise <c>default</c>(<typeparamref name="T"/>).</returns>
        T Get<T>(object key);

        /// <summary>
        /// Attempts to retrieve an object of type <typeparamref name="T"/> from the collection.
        /// </summary>
        /// <typeparam name="T">The type of object that is expected to be found stored in the collection.</typeparam>
        /// <param name="key">The key that the object is stored under.</param>
        /// <param name="value">When this method returns, if the value is found, will contain the value of <typeparamref name="T"/> that was stored in the collection; otherwise <c>default</c>(<typeparamref name="T"/>). This parameter is passed uninitialized.</param>
        /// <returns>If an object of <typeparamref name="T"/> is found, <c>true</c>; otherwise <c>false</c>.</returns>
        bool TryGet<T>(object key, out T value);

        /// <summary>
        /// Stores a value of type <typeparamref name="T"/> in the collection.
        /// </summary>
        /// <typeparam name="T">The type of object to be stored in the collection.</typeparam>
        /// <param name="key">The key that the object is to be stored under.</param>
        /// <param name="value">The value to store in the collection.</param>
        /// <returns><paramref name="value"/></returns>
        void Set<T>(object key, T value);

        /// <summary>
        /// Retrieves an object from the collection.
        /// </summary>
        /// <param name="key">The key that the object is stored under.</param>
        /// <returns>If it exists, the stored object; otherwise <c>null</c>.</returns>
        object this[object key] { get; set; }

        /// <summary>
        /// Removes an object from the collection.
        /// </summary>
        /// <param name="key">The key that the object is stored under.</param>
        void Remove(object key);

        /// <summary>
        /// Removes multiple objects from the collection.
        /// </summary>
        /// <param name="keys">The keys that the objects to be removed are stored under.</param>
        void Remove(params object[] keys);

        /// <summary>
        /// Determines if the collection contains an entry for a given key.
        /// </summary>
        /// <param name="key">The key to check the collection for.</param>
        /// <returns></returns>
        bool Contains(object key);

        /// <summary>
        /// Emptys the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the number of objects stored in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the name of the data store.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
    }
}