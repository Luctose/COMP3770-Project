using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridMaster;
using System.Threading;

namespace PathFinding{
    //This class controls the threads
    public class PathfindMaster : MonoBehaviour{
        // Singleton method
        private static PathfindMaster instance;
		
        void Awake(){
            instance = this;
        }
		// For other classes to Instantiate
        public static PathfindMaster GetInstance(){
            return instance;
        }

        // The maximum simultaneous threads we allow to open
        public int MaxJobs = 3;

        // Delegates are a variable that points to a function
        public delegate void PathfindingJobComplete(List<Node> path);
		// List of 
        private List<Pathfinder> currentJobs;
        private List<Pathfinder> todoJobs;

        void Start(){
            currentJobs = new List<Pathfinder>();
            todoJobs = new List<Pathfinder>();
        }
   
        void Update() {
			int i = 0;	// Index
			// While there are threads still working
            while(i < currentJobs.Count){
                if(currentJobs[i].jobDone){
					// Current job is done, remove the thread
                    currentJobs[i].NotifyComplete();
                    currentJobs.RemoveAt(i);
                }
                else{
                    i++;
                }
            }
			// If there are jobs to be done
            if(todoJobs.Count > 0 && currentJobs.Count < MaxJobs){
                Pathfinder job = todoJobs[0];
                todoJobs.RemoveAt(0);	// Job 1 then becomes Job 0
                currentJobs.Add(job);	// Active job

                //Start a new thread
                Thread jobThread = new Thread(job.FindPath);
                jobThread.Start();

                //As per the doc
                //https://msdn.microsoft.com/en-us/library/system.threading.thread(v=vs.110).aspx
                //It is not necessary to retain a reference to a Thread object once you have started the thread. 
                //The thread continues to execute until the thread procedure is complete.				
            }
        }

		// Open a new thread for finding the path
        public void RequestPathfind(Node start, Node target, PathfindingJobComplete completeCallback){
            Pathfinder newJob = new Pathfinder(start, target, completeCallback);
            todoJobs.Add(newJob);	// Add to list
        }
    }
}