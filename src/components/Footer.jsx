import React from 'react';
import "./style.css";

const Footer = () => {
    return (
        <div>
            <footer class="bg-dark text-light pt-4 mt-5">
  <div class="container">
    <div class="row">
      <div class="col-md-6 mb-3">
        <h5>StreamStore</h5>
        <p class="text-muted"><span class="txt">Digital products & games marketplace.</span></p>
      </div>

      <div class="col-md-3 mb-3">
        <h6>Links</h6>
        <ul class="list-unstyled">
          <li><a href="/" class="text-muted text-decoration-none"><span className='txt'>Home</span></a></li>
          <li><a href="#" class="text-muted text-decoration-none"><span className='txt'>Shop</span></a></li>
          <li><a href="#" class="text-muted text-decoration-none"><span className='txt'>Support</span></a></li>
        </ul>
      </div>

      <div class="col-md-3 mb-3">
        <h6>Social</h6>
        <a href="#" class="text-muted me-2"><span className='txt'>Twitter</span></a>
        <a href="#" class="text-muted"><span className='txt'>Discord</span></a>
      </div>
    </div>

    <div class="txt text-center text-muted border-top pt-3">
      <span className='txt'>© 2026 StreamStore</span>
    </div>
  </div>
</footer>
        </div>
    );
}

export default Footer;
