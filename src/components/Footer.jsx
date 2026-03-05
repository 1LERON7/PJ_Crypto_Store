import React from 'react';
import "./style.css";

const Footer = () => {
    return (
        <div>
            <footer className="bg-color text-light pt-4 mt-5">
  <div className="container">
    <div className="row">
      <div className="col-md-6 mb-3">
        <h5>StreamStore</h5>
        <p className="text-muted"><span className="txt">Digital products & games marketplace.</span></p>
      </div>

      <div className="col-md-3 mb-3">
        <h6>Links</h6>
        <ul className="list-unstyled">
          <li><a href="/" className="text-muted text-decoration-none"><span className='txt'>Home</span></a></li>
          <li><a href="#" className="text-muted text-decoration-none"><span className='txt'>Shop</span></a></li>
          <li><a href="#" className="text-muted text-decoration-none"><span className='txt'>Support</span></a></li>
        </ul>
      </div>

      <div className="col-md-3 mb-3">
        <h6>Social</h6>
        <a href="#" className="text-muted me-2"><span className='txt'>Twitter</span></a>
        <a href="#" className="text-muted"><span className='txt'>Discord</span></a>
      </div>
    </div>

    <div className="txt text-center text-muted border-top pt-3">
      <span className='txt'>© 2026 StreamStore</span>
    </div>
  </div>
</footer>
        </div>
    );
}

export default Footer;
