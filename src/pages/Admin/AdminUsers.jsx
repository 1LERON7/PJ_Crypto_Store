import { useEffect, useState } from "react";
import {deleteUser, getUsers} from "../../api/users";
import ModalUser from "../../components/ModalUsers";
import Modal from "react-bootstrap/Modal";

export default function AdminUsers() {
    const [showDelete, setShowDelete] = useState(false);
    const [selectedUserId, setSelectedUserId] = useState(null);

  const [users, setUsers] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);

  const pageSize = 12;

  const loadUsers = async () => {
    const data = await getUsers(page, pageSize);
  setUsers(data.items);

  setTotalCount(data.totalCount);
};

const handleDeleteClick = (id) => {
  setSelectedUserId(id);
  setShowDelete(true);
};

    const confirmDelete  = async (id) => {
        await deleteUser(selectedUserId);

        setUsers(prev => prev.filter(u => u.id !== id));
        setTotalCount(prev => prev - 1);

        setShowDelete(false);

        loadUsers();
    }

  useEffect(() => {
    loadUsers();
  }, [page]);

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <>

      <div className="d-flex justify-content-between align-items-center mb-3">

        <h3 className="mb-0">Users</h3>

        <span className="badge bg-secondary">
          Total: {totalCount}
        </span>


         <button
          className="btn btn-success"
          data-bs-toggle="modal"
          data-bs-target="#createUserModal"
        >
          + Add Product
        </button>

      </div>

     
   

      <table className="table table-striped table-hover align-middle shadow-sm">

        <thead>
          <tr>
            <th>Email</th>
            <th>Role</th>
            <th>Created</th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
  {users.map(u => (
    <tr key={u.id}>
      <td>{u.email}</td>

      <td>
        <span className={`badge ${u.role === "admin" ? "bg-danger" : "bg-primary"}`}>
          {u.role}
        </span>
      </td>

      <td>{new Date(u.created).toLocaleString()}</td>

      <td>
        <div className="btn-group">


          <button className="btn btn-sm btn-outline-warning">Edit</button>
          
          <button 
            className="btn btn-danger btn-sm"
            onClick={() => handleDeleteClick(u.id)}
            >
            Delete
        </button>

        </div>
      </td>

    </tr>
  ))}
</tbody>

      </table>

      <nav className="mt-4">
        <ul className="pagination">

          <li className={`page-item ${page === 1 ? "disabled" : ""}`}>
            <button
              className="page-link"
              onClick={() => setPage(page - 1)}
            >
              Previous
            </button>
          </li>

          <li className="page-item active">
            <span className="page-link">{page}</span>
          </li>

          <li className={`page-item ${page === totalPages ? "disabled" : ""}`}>
            <button
              className="page-link"
              onClick={() => setPage(page + 1)}
            >
              Next
            </button>
          </li>

        </ul>
      </nav>
<ModalUser  onCreated={loadUsers} loadUsers={loadUsers}/>

 <Modal show={showDelete} onHide={() => setShowDelete(false)}>
      <Modal.Header closeButton>
        <Modal.Title>Delete user</Modal.Title>
      </Modal.Header>

      <Modal.Body>
        Are you sure you want to delete this user?
      </Modal.Body>

      <Modal.Footer>
        <button
          className="btn btn-secondary"
          onClick={() => setShowDelete(false)}
        >
          Cancel
        </button>

        <button
          className="btn btn-danger"
          onClick={confirmDelete}
        >
          Delete
        </button>
      </Modal.Footer>
    </Modal>
    </>
  );
}