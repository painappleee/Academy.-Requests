using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace Academy._Requests
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }

    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
    class Program
    {
        static async Task Main()
        {
            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1 - Действия с постами");
                Console.WriteLine("2 - Другие действия");
                Console.WriteLine("q - Выйти");
                Console.Write("Введите номер действия: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nВыберите действие");
                        Console.WriteLine("1 - Создать новый пост");
                        Console.WriteLine("2 - Получить все посты");
                        Console.WriteLine("3 - Обновить пост");
                        Console.WriteLine("4 - Удалить пост");
                        await MenuPosts();
                        break;
                    case "2":
                        Console.WriteLine("\nВыберите действие");
                        Console.WriteLine("1 - Получить список пользователей");
                        Console.WriteLine("2 - Получить посты пользователя");
                        Console.WriteLine("3 - Фильтровать пользователей по email-домену");
                        Console.WriteLine("4 - Получить пользователей, отсортированных по алфавиту");
                        Console.WriteLine("5 - Найти самый длинный пост");
                        Console.WriteLine("6 - Вывести все комментарии, написанные с почт с email-доменом @vance.io");
                        await MenuAnother();
                        break;
                    case "q":
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод, попробуйте снова.");
                        break;
                
                

                }
            }
        }

        static async Task MenuPosts()
        {
            Console.Write("Введите номер действия: ");
            string choice = Console.ReadLine();

            switch (choice)
            {

                case "1":
                    await CreatePost();
                    break;
                case "2":
                    await GetAllPosts();
                    break;
                case "3":
                    Console.Write("Введите ID поста для обновления: ");
                    int postIdUpd = int.Parse(Console.ReadLine());
                    await UpdatePost(postIdUpd);
                    break;
                case "4":
                    Console.Write("Введите ID поста для удаления: ");
                    int postIdDel = int.Parse(Console.ReadLine());
                    await DeletePost(postIdDel);
                    break;
                default:
                    Console.WriteLine("Некорректный ввод, попробуйте снова.");
                    break;
            }
        }

        static async Task MenuAnother()
        {
            Console.Write("Введите номер действия: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await GetUsers();
                    break;
                case "2":
                    Console.Write("Введите ID пользователя для поиска его постов: ");
                    int userId = int.Parse(Console.ReadLine());
                    await GetUserPosts(userId);
                    break;
                case "3":
                    Console.Write("Введите email-домен для фильтрации пользователей: ");
                    string emailDomain = Console.ReadLine();
                    await FilterUsersByEmail(emailDomain);
                    break;
                case "4":
                    await GetOrderedUsers();
                    break;
                case "5":
                    await GetTheLongestPost();
                    break;
                case "6":
                    await GetCommentsWithEmailDomain();
                    break;
                default:
                    Console.WriteLine("Некорректный ввод, попробуйте снова.");
                    break;
            }
        }



        static async Task GetUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://jsonplaceholder.typicode.com/users";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                    Console.WriteLine("Список пользователей:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
                    }
                }
                else { 
                    Console.WriteLine("Ошибка запроса");
                }
            }
        }

        static async Task GetAllPosts()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://jsonplaceholder.typicode.com/posts";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(json);
                    Console.WriteLine("Список постов:");
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"Id: {post.Id}  UserId: {post.UserId}  Title: {post.Title}");
                        Console.WriteLine(post.Body);
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка запроса");
                }
            }
        }
        static async Task GetUserPosts(int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://jsonplaceholder.typicode.com/posts?userId-{userId}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode) 
                { 
                    string json = await response.Content.ReadAsStringAsync();
                    List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(json);
                    Console.WriteLine($"Посты пользователя с ID {userId}:");
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"- {post.Title}");
                    }
                }
            } 
                
        }

        static async Task CreatePost()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://jsonplaceholder.typicode.com/posts";
                var newPost = new Post { UserId = 1, Title = "Новый пост", Body = "Содержимое нового поста" };
                string json = JsonConvert.SerializeObject(newPost);
                StringContent content = new StringContent(json, Encoding.UTF8,"application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Ответ от сервера: {responseJson}");
            }
        }

        static async Task UpdatePost(int postId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://jsonplaceholder.typicode.com/posts/{postId}";
                var updatedPost = new Post { Id = postId, UserId = 1, Title = "Обновленный заголовок", Body = "Обновленное содержимое" };
                string json = JsonConvert.SerializeObject(updatedPost);
                StringContent content = new StringContent (json, Encoding.UTF8,"application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Обновленный пост: {responseJson}");
            }
        }

        static async Task DeletePost(int postId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://jsonplaceholder.typicode.com/posts/{postId}";
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Пост с ID {postId} удален");
                }
                else Console.WriteLine("Ошибка");
            }
        }

        static async Task FilterUsersByEmail(string emailDomain)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://jsonplaceholder.typicode.com/users";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                    var filteredUserd = users.Where(u => u.Email.EndsWith(emailDomain)).ToList();
                    foreach (var user in filteredUserd)
                    { 
                        Console.WriteLine($"{user.Name}  {user.Email}");
                    }
                }
            }
        }

        static async Task GetOrderedUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://jsonplaceholder.typicode.com/users";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                    users = users.OrderBy(u => u.Name).ToList();
                    Console.WriteLine("Список пользователей, отсортированных по алфавиту:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"{user.Name} ({user.Email}) ID: {user.Id}");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка запроса");
                }
            }
        }

        static async Task GetTheLongestPost()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://jsonplaceholder.typicode.com/posts";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(json);
                    var longestPost = posts.OrderByDescending(p=>p.Body.Length).First();
                    Console.WriteLine($"Самый длинный пост:");
                    Console.WriteLine($"UserId: {longestPost.UserId}  Title: {longestPost.Title}  Length: {longestPost.Body.Length}");
                    Console.WriteLine(longestPost.Body);
                   
                }
            }
        }
       
        static async Task GetCommentsWithEmailDomain()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://jsonplaceholder.typicode.com/comments";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(json);
                    var filteredComments = comments.Where(c => c.Email.EndsWith("@vance.io"));
                    foreach (var comment in filteredComments)
                    {
                        Console.WriteLine($"Email: {comment.Email}  PostId: {comment.PostId}  ");
                        Console.WriteLine(comment.Body);
                    }
                }
            }
        }
        /*
         1. Получение всех пользователей и их сортировка по алфавиту
         2. Найти самый длинный пост
         3. Получить все комментарии и отобрать только те у которых почта заканчивается на @vance.io
         4. CRUD
            1. Создать пост
            2. Получить все посты
            3. Обновить пост с ид
            4. Удалить пост с ид
         */
    }
}
