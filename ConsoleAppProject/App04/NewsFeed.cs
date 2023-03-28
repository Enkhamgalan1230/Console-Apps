﻿using ConsoleAppProject.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleAppProject.App04
{
    ///<summary>
    /// The NewsFeed class stores news posts for the news feed in a social network 
    /// application.
    /// 
    /// Display of the posts is currently simulated by printing the details to the
    /// terminal. (Later, this should display in a browser.)
    /// 
    /// This version does not save the data to disk, and it does not provide any
    /// search or ordering functions.
    ///</summary>
    ///<author>
    ///  Michael Kölling and David J. Barnes
    ///  version 0.1
    ///</author> 
    public class NewsFeed
    {
        
        public List<int> Likes;

        public string Author;
        public bool StopLoop { get; set; }

        public bool NoPosts = false;

        public int AvailablePosts { get; set; }

        public int AvailablePostsIndex { get; set; }

        public List<Post> Posts { get; set; }
        public string Search { get; set; }  

        public const string Stringempty = "";

        ///<summary>
        /// Construct an empty news feed.
        ///</summary>
        public NewsFeed()
        {
            Posts = new List<Post>();

            Likes = new List<int>();

        
        }

        ///<summary>
        /// Add a text post to the news feed.
        /// 
        /// @param text  The text post to be added.
        ///</summary>
        public void AddMessagePost(MessagePost message)
        {
            Posts.Add(message);
        }

        ///<summary>
        /// Add a photo post to the news feed.
        /// 
        /// @param photo  The photo post to be added.
        ///</summary>
        public void AddPhotoPost(PhotoPost photo)
        {
            Posts.Add(photo);
        }

        ///<summary>
        /// Show the news feed. Currently: print the news feed details to the
        /// terminal. (To do: replace this later with display in web browser.)
        ///</summary>
        public void Display()
        {
            if (Posts.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  No posts to display   ");
                Console.ForegroundColor = ConsoleColor.Green;

                Console.Clear();
            }
            else
            {
                LoopDisplay();
            }
            
        }

        private void LoopDisplay()
        {
            if (Posts.Count == 0)
            {
                AvailablePostsIndex = 0;
            }

            else
            {
                for (AvailablePostsIndex = 0; AvailablePostsIndex <= Posts.Count; AvailablePostsIndex++)
                {
                    StopLoop = false;

                    //Console.Clear();

                    RepeatFinalPost();

                    if (AvailablePosts < Posts.Count - 1)
                    {
                        ShowPost();
                    }

                    Console.WriteLine();

                    string[] choices = new string[]
                    {
                      " Like \n",
                      " Unlike \n",
                      " Remove this post \n",
                      " Remove all posts \n",
                      " Comment on this post \n",
                      " Next post \n",
                      " Main Menu \n"
                    };

                    int choice = ConsoleHelper.SelectChoice(choices);

                    switch (choice)
                    {
                        case 1:
                            LikePost(Posts[AvailablePostsIndex]);

                            AvailablePostsIndex--;

                            break;

                        case 2:
                            UnlikePost(Posts[AvailablePostsIndex]);

                            AvailablePostsIndex--;

                            break;

                        case 3:
                            RemovePost();

                            AvailablePostsIndex--;

                            break;

                        case 4:
                            RemoveAllPosts();

                            break;

                        case 5:
                            AddComment(Posts[AvailablePostsIndex]);

                            AvailablePostsIndex--;

                            break;

                        case 6:
                            ShowNextPost();

                            break;

                        case 7:
                            Console.Clear();

                            StopLoop = true;

                            break;

                        default:
                            break;
                    }

                    if (StopLoop && NoPosts)
                    {
                        AvailablePosts = 0;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n ===== All posts removed =====\n");
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.WriteLine();

                        break;
                    }

                    else

                    {
                        if (StopLoop)
                        {
                            AvailablePosts = 0;

                            Console.WriteLine();

                            break;
                        }
                    }
                }
            }
        }

        private void ShowPost()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n   ==== Showing {AvailablePosts + 1}/" +  $"{Posts.Count} posts ====");
            Console.ForegroundColor = ConsoleColor.Green;
            Posts[AvailablePostsIndex].Display();
        }

        private void ShowLastPost()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n   ==== Showing {AvailablePosts + 1}/" + $"{Posts.Count} posts ====");
            Console.ForegroundColor = ConsoleColor.Green;
            Posts[AvailablePosts].Display();
        }

        private void RepeatFinalPost()
        {
            if (AvailablePostsIndex == Posts.Count - 1)
            {
                AvailablePosts = Posts.Count - 1;

                AvailablePostsIndex = AvailablePosts - 1;

                ShowLastPost();

                AvailablePostsIndex++;
            }
        }

        public void ShowNextPost()
        {
            if (AvailablePostsIndex < Posts.Count - 1)
            {
                AvailablePosts++;
            }
            else
            {
                AvailablePostsIndex--;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n ==== No posts to display ! ====\n");
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        public static void AddComment(Post post)
        {
            Console.Write("\n    Enter comment :  ");

            string text = Console.ReadLine();

            post.AddComment(text);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n    -- Comment added --\n");
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private void LikePost(Post post)
        {
            if (Posts[AvailablePostsIndex].Username == NetworkApp.CurrentUser)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n ===== You can't like your own posts ====== \n");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            else
            {
                if (!Likes.Contains(AvailablePostsIndex))
                {
                    post.Like();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n ===== You liked this post ====== \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Likes.Add(AvailablePostsIndex);
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n ===== You've liked this post already ====== \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }

        }

        private void UnlikePost(Post post)
        {
            if (Posts[AvailablePostsIndex].Username == NetworkApp.CurrentUser)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n ===== You cannot like your own posts  ====== \n");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            else
            {
                if (Likes.Contains(AvailablePostsIndex))
                {
                    post.Unlike();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n ===== You unliked this post ====== \n");
                    Console.ForegroundColor = ConsoleColor.Green;

                    int like = Likes.FindIndex(x => x == AvailablePostsIndex);

                    Likes.RemoveAt(like);
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n ===== You didn't like this post ====== \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
        }

        private void RemoveAllPosts()
        {
            Posts = new List<Post>();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  ==== All posts removed ==== \n");
            Console.ForegroundColor = ConsoleColor.Green;


            StopLoop = true;
        }

        private void RemovePost()
        {
            if (Posts[AvailablePostsIndex].Username == NetworkApp.CurrentUser)
            {
                Posts.RemoveAt(AvailablePostsIndex);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n ===== Post removed ====== \n");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            else
            {
                if (Posts.Count == 1)
                {
                    RemoveAllPosts();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n ===== Only the user can remove this post ====== \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }

        }
    
    }
}


