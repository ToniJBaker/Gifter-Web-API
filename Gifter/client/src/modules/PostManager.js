import React from "react";

const baseUrl = '/api/post';

export const getAllPosts = () => {
  return fetch(`${baseUrl}/GetWithComments`) //http GET request or  `/api/post/GetWithComments`
    .then((res) => res.json())
};

export const addPost = (singlePost) => { //http POST request
  return fetch(baseUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(singlePost),
  });
};

export const searchPosts = (query)=> { //http GET by Search `/api/Post/search?q=<query>`
  return fetch(`${baseUrl}/search?q=${query}`)
  .then((res)=> res.json())
};


export const getPost = (id) => {  //http GET by id parameter 
  return fetch(`/api/post/GetWithComments/${id}`).then((res) => res.json());
};