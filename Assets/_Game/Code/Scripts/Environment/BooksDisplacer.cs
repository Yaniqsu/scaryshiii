using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = System.Random;

public class BooksDisplacer : MonoBehaviour
{
    [SerializeField] private GameObject[] books;
    
    [Button("Hide Books")]
    private void DisplaceBooks()
    {
        var seed = Mathf.RoundToInt(Mathf.Pow(transform.position.magnitude, 4) % 4 * 100) * gameObject.name.GetHashCode();
        Debug.Log($"Seed: {seed} {transform.position.magnitude}");
        var random = new Random(seed);

        var booksToHide = random.Next(0, this.books.Length);
        var booksList = new List<GameObject>(books);

        for (int i = 0; i < booksToHide; i++)
        {
            var index = random.Next(0, booksList.Count - 1);
            
            booksList[index].SetActive(false);
            booksList.RemoveAt(index);
        }
        
        booksList.ForEach(book => book.SetActive(true));
    }
    
    [Button("Hide ALL Books")]
    private void DisplaceAllBooks()
    {
        var allDisplacers = FindObjectsByType<BooksDisplacer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var displacer in allDisplacers)
        {
            displacer.DisplaceBooks();
        }
    }
}
