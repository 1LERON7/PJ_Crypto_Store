import React from 'react';
import { useState } from "react";
import { createUser } from "../api/users";
import { Modal } from "bootstrap";

const ModalUsers = ({loadUsers}) => {
    const [email, setEmail] = useState("");
    const [role, setRole] = useState("");
    const [password, setPassword] = useState("");
     const [error, setError] = useState(null);

    const handleCreateUser = async (e) => {
    e.preventDefault();

    const emailParts = email.split("@");

    if (emailParts[0].length < 3) {
      setError("Email must have at least 3 characters before @");
      return;
    }
    if(!email.trim()){
      setError("Email is required");
      return;
    }
    if (!/\S+@\S+\.\S+/.test(email)) {
    setError("Invalid email format");
    return;
  }
  if (password.length < 6) {
    setError("Password must be at least 6 characters");
    return;
  }
  if(email.length > 100){
    setError("Email is so long");
    return;
  }
  if (!/[A-Za-z]/.test(password) || !/[0-9]/.test(password)) {
    setError("Password must contain letters and numbers");
    return;
  }

    await createUser({
        email,
        role,
        password,
    });

    loadUsers();

    // тут короче принудительно закрываем модалку после проверок
    const modal = document.getElementById("createUserModal");
    const modalInstance = Modal.getInstance(modal);
    modalInstance.hide();
};

  


    return (
        <div>
            <div className="modal fade" id="createUserModal">
  <div className="modal-dialog">
    <div className="modal-content">

      <div className="modal-header">
        <h5 className="modal-title">Create User</h5>
        <button className="btn-close" data-bs-dismiss="modal"></button>
      </div>

      <div className="modal-body">

{error && (
    <div className="alert alert-danger">
      {error}
    </div>
  )}
        <div className="mb-3">
          <label className="form-label">Email</label>
          <input
            className="form-control"
            value={email}
            onChange={e => setEmail(e.target.value)}
          />
        </div>

        <select
            className="form-select"
            value={role}
            onChange={(e) => setRole(e.target.value)}
            >
            <option value="">Select role</option>
            <option value="user">User</option>
            <option value="admin">Admin</option>
        </select>

        <div className="mb-3">
            <label className="form-label">Password</label>
            <input
                type="password"
                className="form-control"
                value={password}
                onChange={e => setPassword(e.target.value)}
            />
            </div>

      </div>

      <div className="modal-footer">
        <button className="btn btn-secondary" data-bs-dismiss="modal">
          Cancel
        </button>

        <button
          className="btn btn-success"
          onClick={handleCreateUser}
        //   data-bs-dismiss="modal"
        >
          Create
        </button>
      </div>

    </div>
  </div>
</div>
        </div>
    );
}

export default ModalUsers;
