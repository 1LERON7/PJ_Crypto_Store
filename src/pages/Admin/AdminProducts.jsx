import { useEffect, useState } from "react";
// import { getProducts } from "../api/products";


export default function AdminProducts() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    getProducts.get("/products")
  .then(res => {
    console.log(res.data);
    setProducts(res.data);
  });
  }, []);

  return (
    <>
      <h3>Products</h3>

      <table className="table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Price</th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
          {products.map(p => (
            <tr key={p.id}>
              <td>{p.title}</td>
              <td>${p.price}</td>
              <td>
                <button className="btn btn-sm btn-warning me-2">Edit</button>
                <button className="btn btn-sm btn-danger">Delete</button>
              </td>
            </tr>
          ))}
        </tbody>

      </table>
    </>
  );
}