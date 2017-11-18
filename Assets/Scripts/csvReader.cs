using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;


public class csvReader : MonoBehaviour {

	public string file;
	public static List<string> csvList  = new List<string>();
	public IntroSceneManager sceneManager;

	// Use this for initialization
	void Start () {
		Load (file, csvList);
		sceneManager.OnCsvRead ();
	}
	
	// Update is called once per frame
	void Update () {
			
	}



	private bool Load(string fileName, List<string> arrayToTransferTo) {
	// Handle any problems that might arise when reading the text
		try {
				
			string line;

			// Create a new StreamReader, tell it which file to read and what encoding the file was saved as
			StreamReader csvFileReader = new StreamReader("./Lists/" + fileName + ".csv", Encoding.Default);

			/*/// Immediately clean up the reader after this block of code is done.
			You generally use the "using" statement for potentially memory-intensive objects
			instead of relying on garbage collection. (Do not confuse this with the using 
			directive for namespace at the beginning of a class!) *////
			using (csvFileReader) {

				line = csvFileReader.ReadLine();
					
				if(line != null) {
					
						// While there's lines left in the text file, do this:
						do	{
						//  Do whatever you need to do with the text line, it's a string now. 
							string[] entries = line.Split(',');

							if (entries.Length > 0){
								//Debug.Log(entries[0]);
								arrayToTransferTo.Add (entries[0]);
							}
							//DoStuff(entries);
							line = csvFileReader.ReadLine();
						
							}

						while (line != null);
					} 

					// Done reading, close the reader and return true to broadcast success    
					csvFileReader.Close();

					return true;
				}
			}

		
		// If anything broke in the try block, we throw an exception with information on what didn't work
		catch (System.Exception e) {
			Debug.Log("{0}\n" + e.Message);
			return false;
		}
	}


}

