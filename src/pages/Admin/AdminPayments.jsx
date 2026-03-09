import React, { useEffect, useState } from 'react';
import api from '../../api/api';

const AdminPayments = () => {
    const [payments, setPayments] = useState([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);

    const [totalAmount, setTotalAmount] = useState(0);
    const [averageAmount, setAverageAmount] = useState(0);

    const pageSize = 12;

  const loadPayments = () => {
    api.get(`/payments?page=${page}&pageSize=${pageSize}`).then(({data}) => {
    setPayments(data.items ?? []);

    setTotalCount(data.totalCount);
    setTotalAmount(data.totalAmount);
    setAverageAmount(data.averageAmount);
    })
  } 

  useEffect(() => {
  loadPayments();
}, [page]);

const totalPages = Math.ceil(totalCount / pageSize);

    return (
        <>

      <div className="d-flex justify-content-between align-items-center mb-3">
        <h3 className="mb-0">Payments</h3>
      </div>

<div className="row g-3 mb-4">

  <div className="col-md-4">
    <div className="card border-0 shadow-sm bg-body-tertiary">
      <div className="card-body">
        <div className="text-muted middle">💳 Total Payments</div>
        <div className="fs-3 fw-bold">
          {totalCount}
        </div>
      </div>
    </div>
  </div>

  <div className="col-md-4">
    <div className="card border-0 shadow-sm bg-body-tertiary">
      <div className="card-body">
        <div className="text-muted middle">💰 Total Earned</div>
        <div className="fs-3 fw-bold text-success">
          ETH {(totalAmount ?? 0).toFixed(2)}
        </div>
      </div>
    </div>
  </div>

  <div className="col-md-4">
    <div className="card border-0 shadow-sm bg-body-tertiary">
      <div className="card-body">
        <div className="text-muted middle">📊 Average Payment</div>
        <div className="fs-3 fw-bold text-warning">
          ETH {(averageAmount ?? 0).toFixed(2)}
        </div>
      </div>
    </div>
  </div>

</div>

      <table className="table table-striped table-hover align-middle shadow-sm">

        <thead>
          <tr>
            <th>User</th>
            <th>Product</th>
            <th>Amount</th>
            <th>TxHash</th>
            <th>Date</th>
          </tr>
        </thead>

        <tbody>
          {payments.map(p => (
            <tr key={p.id}>

              <td className="fw-semibold text-primary">
                {p.userEmail}
              </td>

              <td className="text-truncate title-cell">
                {p.productTitle}
              </td>

              <td className="text-success fw-bold">
                ETH {p.amount}
              </td>

              <td className="font-monospace">
                {p.txHash?.slice(0, 10)}...
              </td>

              <td>
                {new Date(p.created).toLocaleString()}
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

    </>
    );
}

export default AdminPayments;
