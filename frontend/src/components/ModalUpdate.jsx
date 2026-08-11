import React, { useEffect, useState } from 'react';

const ModalUpdate = ({ product, onSave }) => {
    const [form, setForm] = useState(product || {});

    const handleUpdate = (e) => {
      const {name, value} = e.target;

      console.log(form);
      console.log(typeof form.price);

        setForm({
            ...form,
            [name]: value
        })
    }

    useEffect(() => {
  if (product) {
    setForm(product);
  }
}, [product]);


    const handleSave = () => {
        console.log(form);
        onSave({
          ...form,
          price: Number(form.price)
        });
    };

    
    return (
       <div className="modal fade" id="updateProductModal">
      <div className="modal-dialog">
        <div className="modal-content">

          <div className="modal-header">
            <h5 className="modal-title">Edit Product</h5>
            <button className="btn-close" data-bs-dismiss="modal"></button>
          </div>

          <div className="modal-body">

            <div className="mb-3">
              <label className="form-label">Title</label>

            <input
            name="title"
            className="form-control"
            value={form.title || ""}
            onChange={handleUpdate}
            />

            </div>

            <div className="mb-3">
              <label className="form-label">Price</label>
              <input
                name="price"
                type="number"
                className="form-control"
                value={form.price || ""}
                onChange={handleUpdate}
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Description</label>
              <textarea
                name="description"
                className="form-control"
                value={form.description || ""}
                onChange={handleUpdate}
                />
            </div>

            <div className="mb-3">
              <label className="form-label">Image URL</label>

              <input
                name="imageUrl"
                className="form-control"
                value={form.imageUrl || ""}
                onChange={handleUpdate}
              />

            </div>

          </div>

          <div className="modal-footer">
            <button className="btn btn-secondary" data-bs-dismiss="modal">
              Cancel
            </button>

            <button
              className="btn btn-success"
              onClick={handleSave}
              data-bs-dismiss="modal"
            >
              Save
            </button>
          </div>

        </div>
      </div>
    </div>
  );
}

export default ModalUpdate;
