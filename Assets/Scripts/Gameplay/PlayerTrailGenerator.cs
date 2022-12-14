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

        private SpriteRenderer baseRenderer;
        private Collider2D playerCollider;
        private SpriteRenderer playerRenderer;

        private static int logCount = 0;

        // Start is called before the first frame update
        void Awake()
        {
            playerCollider = player.GetComponent<Collider2D>();
            playerRenderer = player.GetComponent<SpriteRenderer>();
            baseRenderer = baseTrail.GetComponent<SpriteRenderer>();
            baseRenderer.color = playerRenderer.color;

            // Create the player trail as a separate GameObject
            baseTrail.transform.parent = null;
            Collider2D baseCollider = baseTrail.GetComponent<Collider2D>();
            baseCollider.enabled = false;
            Physics2D.IgnoreCollision(baseCollider, playerCollider);

            baseRenderer.enabled = false;
            GameObject newTrail = Instantiate<GameObject>(baseTrail);
            Collider2D newTrailCollider = newTrail.GetComponent<Collider2D>();
            newTrailCollider.enabled = true;
            Physics2D.IgnoreCollision(newTrailCollider, playerCollider, true);
            newTrail.GetComponent<SpriteRenderer>().enabled = true;
            //baseTrail.tag = string.Format("{0} Trail", this.name is string playerTag && playerTag != null ? playerTag : "Indefinted");
            player.trail.Add(newTrail);
        }

        // Update is called once per frame
        void Update()
        {
            // Upon reaching the movePoint, the player's trail will extend to the player's current location
            if (this.player.updateTrail)
            {
                if (this.player.transform.position != this.player.trail[this.player.trail.Count - 1].transform.position)
                {
                    this.HandleTrail(this.player.trail, this.baseTrail, this.player, this.baseRenderer.bounds.size);
                }
                player.updateTrail = false;
            }
        }

        private void HandleTrail(List<GameObject> trail, GameObject baseTrail, 
            Player player, Vector3 size)
        {
            if (player.directionsMatch)
            {
                TestUtil.Log("Updating Trail: " + logCount);
                GameObject lastTrail = trail[trail.Count - 1];
                this.UpdateLastTrail(lastTrail, player.bufferDir, size);
            }
            else
            {
                TestUtil.Log("Creating Trail: " + logCount);
                this.CreateTrail(trail, baseTrail, player.transform.position);
            }
            logCount++;
        }

        private void CreateTrail(List<GameObject> trail, GameObject baseTrail, Vector3 position)
        {
            if (trail[trail.Count - 1] is GameObject lastTrail && lastTrail.transform.position != position)
            {
                GameObject newTrail = Instantiate<GameObject>(baseTrail);
                newTrail.transform.position = position;
                newTrail.GetComponent<SpriteRenderer>().enabled = true;
                Collider2D newCollider = newTrail.GetComponent<Collider2D>();
                newCollider.enabled = true;
                Physics2D.IgnoreCollision(lastTrail.GetComponent<Collider2D>(), playerCollider, false);
                Physics2D.IgnoreCollision(newCollider, playerCollider);

                //trail.ForEach(t => Physics2D.IgnoreCollision(t.GetComponent<Collider2D>(), newCollider));
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