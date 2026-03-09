import { useEffect, useState } from "react";
import api from "../../api/api";
import "./admin.css";
import CreateProductModal from "../../components/ModalProduct";
import UpdateProductModal from "../../components/ModalUpdate";
import { deleteProduct } from "../../api/products";


export default function AdminProducts() {
  const [selectedProduct, setSelectedProduct] = useState(null);

  const [products, setProducts] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);

  const pageSize = 12;

  const loadProducts = () => {
    api.get(`/products?page=${page}&pageSize=${pageSize}`).then(({data}) => {
    setProducts(data.items ?? []);
    setTotalCount(data.totalCount);
    
    })
  } 

  useEffect(() => {
  loadProducts();
}, [page]);


  const handleUpdate = async (product) => {
  
  const dto = {
    title: product.title,
    price: Number(product.price),
    description: product.description,
    imageUrl: product.imageUrl
  };

  await api.put(`/products/${product.id}`, dto);
  
  loadProducts();
};

  const handleDelete = async (id) => {
    await deleteProduct(id);

    setProducts(prev => 
      prev.filter(p => p.id !== id)
    );
    setTotalCount(prev => prev - 1);
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <>

      <div className="d-flex justify-content-between align-items-center mb-3">
        <h3 className="mb-0">Products</h3>

        <span className="badge bg-secondary">
          Total: {totalCount}
        </span>
        
        <button
          className="btn btn-success"
          data-bs-toggle="modal"
          data-bs-target="#createProductModal"
        >
          + Add Product
        </button>
      </div>
      
     

      <table className="table table-striped table-hover align-middle shadow-sm">
        <thead>
          <tr>
            <th>Title</th>
            <th>Price</th>
            <th>Descrition</th>
            <th>Created</th>
            <th>Actions</th>
            
          </tr>
        </thead>

        <tbody>
          {products.map(p => (
            <tr key={p.id}>
              <td className="text-truncate title-cell">
              {p.title}
              </td>
              <td>ETH {p.price}</td>

              <td className="text-truncate description-cell">
                {p.description}
              </td>

              <td>{new Date(p.created).toLocaleString()}</td>
              

              <td>
              <div className="btn-group">

                <button 
                className="btn btn-sm btn-outline-warning"
                onClick={() => {
                  setSelectedProduct(p);
                }}
                data-bs-toggle="modal"
                data-bs-target="#updateProductModal"

                >Edit</button>


                <button 
                className="btn btn-danger btn-sm"
                onClick={() => handleDelete(p.id)}
                >Delete</button>
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
      <span className="page-link">
        {page}
      </span>
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

<CreateProductModal onCreated={loadProducts} />
<UpdateProductModal product={selectedProduct} onSave={handleUpdate}/>
    </>
    
  );
}