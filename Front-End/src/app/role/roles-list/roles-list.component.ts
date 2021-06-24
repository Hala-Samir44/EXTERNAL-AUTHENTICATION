import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AddRoleComponent } from '../add-role/add-role.component';

@Component({
  selector: 'app-roles-list',
  templateUrl: './roles-list.component.html',
  styleUrls: ['./roles-list.component.scss']
})
export class RolesListComponent implements OnInit {
  Roles: any[] = [];
  Permissions: any[] = [];
  perlenth=0;
  @ViewChild(AddRoleComponent) child:any;
  isEdit=false;
  EditRoleID=0;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getAllRoles();
    this.getAllPermissions();
  }
  getAllRoles() {
    this.http.get('https://localhost:44307/api/Role/GetAllRoles').subscribe(
      (res: any) => {
        console.log("Ss", res);
        this.Roles = res.data;
        console.log("this.Roles", this.Roles);
      }
    )
  }
  roleAdded(event: any) {

    if(this.isEdit){
      var index = this.Roles.indexOf(this.Roles.find(e=>e.id == this.EditRoleID));
      this.Roles[index]=event;
    }else{
      this.Roles.push(event)
    }
  }
  EditRoleModal(id:any){
    console.log("df",id);
    this.EditRoleID= id;
    var role = this.Roles.find(e=>e.id == id);
    console.log("d",role);
    this.child.doSomething(role);
    this.isEdit=true;
  }

  DeleteRole(id:any){
    
   var data={
      id:id
    }
    this.http.post('https://localhost:44307/api/Role/DeleteRole', data).subscribe(
      (res: any) => {
        if (res.status == 1) {
          var index = this.Roles.indexOf(this.Roles.find(e=>e.id == id));
      this.Roles.splice(index,1);
        }
        console.log("this.res", res);
      }
    )

  }

  getAllPermissions() {
    this.http.get('https://localhost:44307/api/Permission/GetAllPermissions').subscribe(
      (res: any) => {
        this.Permissions = res.data;
        this.perlenth= this.Permissions.length;
        console.log("this.Permissionllls", this.Permissions,this.perlenth);
      }
    )
  }
 
}
