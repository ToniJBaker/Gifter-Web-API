import React, { useState, useEffect } from "react";
import Post from './Post';
import { getAllPosts, searchPosts } from "../modules/PostManager";

const PostList = () => {
  const [posts, setPosts] = useState([]);
  const [query, setQuery] = useState("");

  const getPosts = () => {
    getAllPosts().then(allPosts => setPosts(allPosts)); //using fetch call getAllPosts()
  };

  const searchAllPosts = (e) => { 
    e.preventDefault()  //necessary because using a form element, not necessary if using <section>  
    searchPosts(query).then(post => setPosts(post));
  };

  


  useEffect(() => {
    getPosts();
  }, []); //empty array [] means it will run once when page loads

  return (<>
    
    <h2 className="welcome">Search Posts</h2>
      <form className="row g-3" >
        <div>
          <input className="form-control" property="search" onChange={e => setQuery(e.target.value)} placeholder="Enter Key Word"/>
        </div>
        <button onClick={searchAllPosts}  className="btn btn-primary">Submit</button>
      </form>
    <div className="container">
      <div className="row justify-content-center">
        <div className="cards-column">
          {posts.map((post) => (
            <Post key={post.id} post={post} />  //using key and prop
          ))}
        </div>
      </div>
    </div>
    </>
  );
};

export default PostList;