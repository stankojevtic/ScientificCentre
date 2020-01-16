import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-list',
  templateUrl: './admin-list.component.html',
  styleUrls: ['./admin-list.component.scss']
})
export class AdminListComponent implements OnInit {
  usersWaitingApproval = null;
  magazinesWaitingReview = null;
  usersHeadElements = ['#', 'Username', 'Firstname', 'Lastname', 'City', 'Country', 'Vocation', 'Scientific Areas', 'Action'];
  magazineHeadElements = ['#', 'Name', 'ISSN', 'IsOpenAccess', 'Scientific Areas', 'Editors', 'Reviewers', 'Chief editor', 'Action'];

  constructor(private httpClient: HttpClient, private router: Router, private toastrService: ToastrService)  { }

  ngOnInit() {
    this.refreshTasks();
  }

  refreshTasks() {
    this.httpClient.get('https://localhost:44372/api/user/getReviewersWaiting').subscribe(
      (res) => {
        this.usersWaitingApproval = res;
      },
      (error) => {
        console.log('Could not get admin tasks.');
      }
    )

    this.httpClient.get('https://localhost:44372/api/admintask/getMagazinesWaitingForApproval').subscribe(
      (res) => {
        this.magazinesWaitingReview = res;
      },
      (error) => {
        this.toastrService.error('Could not get admin tasks.', 'Error', { progressBar: true });
      }
    )
  }

  onApprove(id: Number) {
    this.httpClient.post(`https://localhost:44372/api/user/approve/${id}`, null).subscribe(
      (res) => {
        this.toastrService.success('Successfully approved user as reviewer.', 'Success', { progressBar: true });
        this.router.navigate(['home']);
      },
      (err) => {
        this.toastrService.error('Could not approve user as reviewers.', 'Error', { progressBar: true });
        this.router.navigate(['home']);
      }
    )
  }

  onApproveMagazine(id: Number) {
    this.httpClient.post(`https://localhost:44372/api/admintask/approve/${id}`, null).subscribe(
      (res) => {
        this.toastrService.success('Successfully approved magazine data.', 'Success', { progressBar: true });
        this.router.navigate(['home']);
      },
      (err) => {
        this.toastrService.error('Could not approve magazine data.', 'Error', { progressBar: true });
        this.router.navigate(['home']);
      }
    )
  }

  onDeclineMagazine(id: Number) {
    this.httpClient.post(`https://localhost:44372/api/admintask/decline/${id}`, null).subscribe(
      (res) => {
        this.toastrService.success('Successfully declined magazine data.', 'Success', { progressBar: true });
        this.router.navigate(['home']);
      },
      (err) => {
        this.toastrService.error('Could not decline magazine data.', 'Error', { progressBar: true });
        this.router.navigate(['home']);
      }
    )
  }

}
