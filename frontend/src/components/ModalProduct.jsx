import { useState } from "react";
import { createProduct } from "../../src/api/products";

export default function CreateProductModal({ onCreated }) {
    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");
    const [description, setDescription] = useState("");
    const [imageUrl, setImageUrl] = useState("");

  const handleCreate = async () => {
    await createProduct({
      title,
      price,
      description,
      imageUrl,
    });
    

    onCreated(); // обновление списка продуктов
  };

  return (
    <div className="modal fade" id="createProductModal">
      <div className="modal-dialog">
        <div className="modal-content">

          <div className="modal-header">
            <h5 className="modal-title">Create Product</h5>
            <button className="btn-close" data-bs-dismiss="modal"></button>
          </div>

          <div className="modal-body">

            <div className="mb-3">
              <label className="form-label">Title</label>
              <input
                className="form-control"
                value={title}
                onChange={e => setTitle(e.target.value)}
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Price</label>
              <input
                type="number"
                className="form-control"
                value={price}
                onChange={e => setPrice(e.target.value)}
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Description</label>
              <textarea
                className="form-control"
                rows="3"
                value={description}
                onChange={e => setDescription(e.target.value)}
              />
            </div>

            <div className="mb-3">
            <label className="form-label">Image URL</label>
            <input
                className="form-control"
                value={imageUrl}
                onChange={e => setImageUrl(e.target.value)}
            />
            </div>

          </div>

          <div className="modal-footer">
            <button className="btn btn-secondary" data-bs-dismiss="modal">
              Cancel
            </button>

            <button
              className="btn btn-success"
              onClick={handleCreate}
              data-bs-dismiss="modal"
            >
              Create
            </button>
          </div>

        </div>
      </div>
    </div>
  );
}