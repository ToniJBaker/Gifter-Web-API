
 export const getCurrentUser = () => {
    const currentUser = localStorage.getItem("gifterUser");

    return currentUser;
  };
 

  export const login = (userObject) => {
    
   return fetch(`/api/userprofile/getbyemail?email=${userObject.email}`)
      .then((r) => r.json())
      .then((userObjFromDB) => {
        
        localStorage.setItem("gifterUser", JSON.stringify(userObjFromDB));
    
      })
  };

 export const register = (userObject) => {
    return fetch("/api/userprofile", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userObject),
    })
      .then((response) => response.json())
      .then((userObject) => {
        localStorage.setItem("gifterUser", JSON.stringify(userObject));
      });
  };

 export const logout = () => {
    localStorage.clear();
  };
