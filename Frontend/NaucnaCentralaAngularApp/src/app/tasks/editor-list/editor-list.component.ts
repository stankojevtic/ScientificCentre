import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-editor-list',
  templateUrl: './editor-list.component.html',
  styleUrls: ['./editor-list.component.scss']
})
export class EditorListComponent implements OnInit {
  addingReviewersTask = null;
  correctMagazineTask = null;
  headElements = ['#', 'Name', 'ISSN', 'IsOpenAccess', 'Scientific Areas', ''];

  constructor(private httpClient: HttpClient, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.refreshTasks();
  }

  refreshTasks() {
    this.httpClient.get('https://localhost:44372/api/editortask/getAddingReviewersTask').subscribe(
      (res) => {
        this.addingReviewersTask = res;
      },
      (error) => {

      }
      
    )

    this.httpClient.get('https://localhost:44372/api/admintask/getMagazinesForCorrection').subscribe(
      (res) => {
        this.correctMagazineTask = res;
      },
      (error) => {

      }
      
    )
  }

  onAdd(id: Number) {
    this.router.navigate(['../', 'add-reviewers-and-editors', id], {relativeTo: this.route});
  }

  onCorrect(id: Number) {
    this.router.navigate(['../', 'correct-magazine-data', id], {relativeTo: this.route});
  }

}
