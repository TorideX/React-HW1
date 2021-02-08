import React, { useState, useEffect } from "react";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
  UncontrolledDropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
  NavbarText,
} from "reactstrap";

export const MyNavbar = ({follows}) => {
  const [isOpen, setIsOpen] = useState(false);
  const toggle = () => setIsOpen(!isOpen);

  useEffect(()=>{
    console.log(follows)
  }, [follows])

  return (
    <div>
      <Navbar color="light" light expand="md">
        <NavbarBrand href="/">Developers</NavbarBrand>
        <NavbarToggler onClick={toggle} />
        <Collapse isOpen={isOpen} navbar>
          <Nav className="mr-auto" navbar>
            <NavItem>
              <NavLink href="/components/">Components</NavLink>
            </NavItem>
            <NavItem>
              <NavLink href="">
                GitHub
              </NavLink>
            </NavItem>
          </Nav>
            <UncontrolledDropdown nav inNavbar>
              <DropdownToggle nav caret>
                Follows
              </DropdownToggle>
              <DropdownMenu right>
                  {
                      follows.map((dev, i) => (
                        <DropdownItem key={i}>{dev.firstName} {dev.lastName} {dev.age}</DropdownItem>
                      ))
                  }
              </DropdownMenu>
            </UncontrolledDropdown>
        </Collapse>
      </Navbar>
    </div>
  );
};
