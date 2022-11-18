import { useEffect, useState } from "react"
import { CardTitle } from "reactstrap"
import PostList from "./PostList";
import Post from "./Post";
import { addPost } from "../modules/PostManager";
import { Navigate, useNavigate } from "react-router-dom";


export const PostForm = ({getPosts}) => {

    const [newPost, setNewPost] = useState({
       title: "",
       imageUrl: "",
       caption: "",
       userProfileId: 0,
       dateCreated : 0
    })
    const navigate = useNavigate();
    
    
    const handleSaveNewPost = (event) => {
        event.preventDefault()
        const postToSendToAPI = {
            title: newPost.title,
            imageUrl: newPost.imageUrl,
            caption: newPost.caption,
            userProfileId: 1,
            dateCreated: new Date()
        }
        return (
            addPost(postToSendToAPI)  // navigation correct?
             .then((p)=> {
                navigate("/");
             })
        )
    }
    
    const saveNewPost = (evt) => {
        const copy = {...newPost}
        copy[evt.target.id] = evt.target.value
        setNewPost(copy)
    }

    return (
        <>
            <h2 className="welcome">Add New Post</h2>
            <form className="row g-3" onSubmit={handleSaveNewPost}>
                <div className= "col-md-6">
                    <label htmlFor="title" className="form-label">Title</label>
                    <input type="text" onChange={saveNewPost} className="form-control" id="title"/>
                </div>
                <div className= "col-md-6">
                    <label htmlFor="imageUrl" className="form-label">Image Url</label>
                    <input type="text" onChange={saveNewPost} className="form-control" id="imageUrl"/>
                </div>
                <div className= "col-md-6">
                    <label htmlFor="caption" className="form-label">Caption</label>
                    <input type="text" onChange={saveNewPost} className="form-control" id="caption"/>
                </div>
                
                <button type="submit" className="btn btn-primary">Save</button>
            </form>
        
        
        </>
    )

}
export default PostForm;