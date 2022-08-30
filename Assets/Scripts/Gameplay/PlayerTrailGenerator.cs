using System.Collections.Generic;
using UnityEngine;
using Navigation;
using System;
using Util;

namespace Gameplay
{
    public class PlayerTrailGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject baseTrail;
        [SerializeField] private Player player;

        private readonly List<GameObject> trail = new(); // Holds all the create trail objects created when a user passes a tile
        private SpriteRenderer baseRenderer;

        // Start is called before the first frame update
        void Awake()
        {
            baseRenderer = baseTrail.GetComponent<SpriteRenderer>();

            // Create the player trail as a separate GameObject
            baseTrail.transform.parent = null;
            baseRenderer.enabled = false;
            GameObject newTrail = Instantiate<GameObject>(baseTrail);
            newTrail.GetComponent<SpriteRenderer>().enabled = true;
            //baseTrail.tag = string.Format("{0} Trail", this.name is string playerTag && playerTag != null ? playerTag : "Indefinted");
            trail.Add(newTrail);
        }

        private static int count = 0; 

        // Update is called once per frame
        void Update()
        {
            // Upon reaching the movePoint, the player's trail will extend to the player's current location
            if (player.updateTrail)
            {
                if (player.transform.position != trail[trail.Count - 1].transform.position)
                {
                    this.HandleTrail(this.trail, this.baseTrail, this.player, this.baseRenderer.bounds.size);
                    count++;
                }
                player.updateTrail = false;
            }
        }

        private void HandleTrail(List<GameObject> trail, GameObject baseTrail, 
            Player player, Vector3 size)
        {
            if (player.directionsMatch)
            {
                TestUtil.Log("Updating Trail: " + count);
                GameObject lastTrail = trail[trail.Count - 1];
                this.UpdateLastTrail(lastTrail, player.bufferDir, size);
            }
            else
            {
                TestUtil.Log("Creating Trail: " + count);
                this.CreateTrail(trail, baseTrail, player.transform.position);
            }
            //this.CreateTrail(trail, baseTrail, position);
        }

        private void CreateTrail(List<GameObject> trail, GameObject baseTrail, Vector3 position)
        {
            if (trail[trail.Count - 1] is GameObject lastTrail && lastTrail.transform.position != position)
            {
                GameObject newTrail = Instantiate<GameObject>(baseTrail);
                newTrail.transform.position = position;
                newTrail.GetComponent<SpriteRenderer>().enabled = true;
                trail.Add(newTrail);
            }
        }

        /// <summary>
        /// Ipdates the position and/sor dimensions of the last trail object created
        /// </summary>
        /// <param name="lastTrail">The last trail GameObject that was created</param>
        /// <param name="bufferedDir">The direction the player will be traveling</param>
        /// <param name="size">Size of the player that will be added to the trail with or length</param>
        private void UpdateLastTrail(GameObject lastTrail, Direction bufferedDir, Vector3 size)
        {
            float deltaPosX;
            float deltaPosY;
            float deltaScaleX;
            float deltaScaleY;
            switch (bufferedDir)
            {
                case Direction.NORTH:
                    deltaScaleX = 0;
                    deltaScaleY = size.y;
                    deltaPosX = 0;
                    deltaPosY = size.y / 2;
                    break;
                case Direction.SOUTH:
                    deltaScaleX = 0;
                    deltaScaleY = size.y;
                    deltaPosX = 0;
                    deltaPosY = -size.y / 2;
                    break;
                case Direction.EAST:
                    deltaScaleX = size.x;
                    deltaScaleY = 0;
                    deltaPosX = size.x / 2;
                    deltaPosY = 0;
                    break;
                case Direction.WEST:
                    deltaScaleX = size.x;
                    deltaScaleY = 0;
                    deltaPosX = -size.x / 2;
                    deltaPosY = 0;
                    break;
                case Direction.NONE:
                    deltaScaleX = 0;
                    deltaScaleY = 0;
                    deltaPosX = 0;
                    deltaPosY = 0;
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        "The entered direction \"{0}\" does not have an implemented bahavior", bufferedDir));
            }

            lastTrail.transform.localScale += new Vector3(deltaScaleX, deltaScaleY, 0);
            lastTrail.transform.position += new Vector3(deltaPosX, deltaPosY, 0f);
        }

    }
}